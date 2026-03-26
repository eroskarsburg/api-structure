using Bank.Shared.Result;

namespace Bank.Application.Payments.QrCodeProviders;

public interface IQrCodeProvider
{
    string ProviderId { get; }

    Task<Result<QrCodeProviderResponse>> GenerateAsync(
        QrCodeGenerationRequest request,
        CancellationToken cancellationToken);
}

public record QrCodeProviderResponse(
    string EmvPayload,
    string QrCodeText);
