using System.Collections.Concurrent;
using Bank.Application.Payments.ProviderSelection;
using Bank.Application.Payments.QrCodeProviders;
using Bank.Shared.Errors;
using Bank.Shared.Result;

namespace Bank.Application.Payments;

public class QrCodeService : IQrCodeService
{
    private readonly ConcurrentDictionary<Guid, QrCodeGenerationResponse> _store = new();
    private readonly IPaymentProviderPriorityResolver _priorityResolver;
    private readonly IQrCodeProviderCatalog _providerCatalog;

    public QrCodeService(
        IPaymentProviderPriorityResolver priorityResolver,
        IQrCodeProviderCatalog providerCatalog)
    {
        _priorityResolver = priorityResolver;
        _providerCatalog = providerCatalog;
    }

    public Task<Result<QrCodeGenerationResponse>> GenerateAsync(QrCodeGenerationRequest request, CancellationToken cancellationToken)
    {
        if (request.CustomerAccountId == Guid.Empty)
        {
            return Task.FromResult(Result<QrCodeGenerationResponse>.BadRequest([
                Error.Create("VALIDATION_ERROR", "CustomerAccountId is required.", "customerAccountId")
            ]));
        }

        if (request.Amount <= 0)
        {
            return Task.FromResult(Result<QrCodeGenerationResponse>.BadRequest([
                Error.Create("VALIDATION_ERROR", "Amount must be greater than zero.", "amount")
            ]));
        }

        if (string.IsNullOrWhiteSpace(request.PixKey))
        {
            return Task.FromResult(Result<QrCodeGenerationResponse>.BadRequest([
                Error.Create("VALIDATION_ERROR", "PixKey is required.", "pixKey")
            ]));
        }

        return GenerateWithFallbackAsync(request, cancellationToken);
    }

    public Task<Result<QrCodeGenerationResponse>> GetByIdAsync(Guid qrCodeId, CancellationToken cancellationToken)
    {
        if (_store.TryGetValue(qrCodeId, out var response))
        {
            return Task.FromResult(Result<QrCodeGenerationResponse>.Success(response));
        }

        return Task.FromResult(Result<QrCodeGenerationResponse>.Fail(
            Error.Create("QRCODE_NOT_FOUND", "QrCode was not found.", "qrCodeId")));
    }

    private async Task<Result<QrCodeGenerationResponse>> GenerateWithFallbackAsync(
        QrCodeGenerationRequest request,
        CancellationToken cancellationToken)
    {
        var providers = await _priorityResolver.ResolvePrioritizedProvidersAsync(request.CustomerAccountId, cancellationToken);

        if (providers.Count == 0)
        {
            return Result<QrCodeGenerationResponse>.Fail(
                Error.Create("PAYMENT_PROVIDER_NOT_CONFIGURED", "No payment providers are configured for this customer account.", "customerAccountId"));
        }

        Error? lastError = null;

        foreach (var providerId in providers)
        {
            if (!_providerCatalog.TryGet(providerId, out var provider))
            {
                lastError = Error.Create("PAYMENT_PROVIDER_NOT_REGISTERED", $"Payment provider '{providerId}' is not registered in the application.");
                continue;
            }

            var providerResult = await provider.GenerateAsync(request, cancellationToken);

            switch (providerResult)
            {
                case Success<QrCodeProviderResponse> success:
                    var createdAtUtc = DateTime.UtcNow;
                    var qrCodeId = Guid.NewGuid();

                    var response = new QrCodeGenerationResponse(
                        qrCodeId,
                        success.Value.EmvPayload,
                        success.Value.QrCodeText,
                        createdAtUtc,
                        request.ExpiresAtUtc);

                    _store[qrCodeId] = response;
                    return Result<QrCodeGenerationResponse>.Success(response);

                case BadRequest<QrCodeProviderResponse> badRequest:
                    var errors = badRequest.Erros.Count > 0
                        ? badRequest.Erros
                        : [Error.Create("QRCODE_PROVIDER_VALIDATION_ERROR", $"Provider '{providerId}' rejected the request.")];
                    return Result<QrCodeGenerationResponse>.BadRequest(errors);

                case Fail<QrCodeProviderResponse> fail:
                    lastError = fail.Erro;
                    break;

                default:
                    lastError = Error.Create("QRCODE_PROVIDER_ERROR", $"Provider '{providerId}' returned an unexpected result.");
                    break;
            }
        }

        return Result<QrCodeGenerationResponse>.Fail(
            lastError ?? Error.Create("QRCODE_GENERATION_FAILED", "Unable to generate a QR code using the configured providers."));
    }
}
