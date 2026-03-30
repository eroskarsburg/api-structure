using Bank.Application.QrCode.v1;
using Bank.Shared.Errors;
using Bank.Shared.Result;

namespace Bank.Application.Payments.QrCodeProviders;

public sealed class FailingQrCodeProvider : IQrCodeProvider
{
    public string ProviderId => "always_fail";

    public Task<Result<QrCodeProviderResponse>> GenerateAsync(
        QrCodeRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(Result<QrCodeProviderResponse>.Fail(
            Error.Create("QRCODE_PROVIDER_ERROR", "Provider is unavailable.")));
    }
}

