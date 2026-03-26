using Bank.Shared.Errors;
using Bank.Shared.Result;

namespace Bank.Application.Payments.QrCodeProviders;

public sealed class PixEmvStaticQrCodeProvider : IQrCodeProvider
{
    public string ProviderId => "pix_emv_static";

    public Task<Result<QrCodeProviderResponse>> GenerateAsync(
        QrCodeGenerationRequest request,
        CancellationToken cancellationToken)
    {
        var createdAtUtc = DateTime.UtcNow;
        var txId = Guid.NewGuid().ToString("N")[..32].ToUpperInvariant();
        var merchantName = string.IsNullOrWhiteSpace(request.PayerName) ? "PIX MERCHANT" : request.PayerName;
        var emvPayload =
            $"00020126360014BR.GOV.BCB.PIX0114{request.PixKey}520400005303986540{request.Amount:0.00}5802BR5913{merchantName}62120508{txId}6304ABCD";

        if (string.IsNullOrWhiteSpace(emvPayload))
        {
            return Task.FromResult(Result<QrCodeProviderResponse>.Fail(
                Error.Create("QRCODE_PROVIDER_ERROR", "Unable to generate EMV payload.")));
        }

        var response = new QrCodeProviderResponse(emvPayload, emvPayload);
        return Task.FromResult(Result<QrCodeProviderResponse>.Success(response));
    }
}

