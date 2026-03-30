using Bank.Application.QrCode.v1;
using Bank.Shared.Result;

namespace Bank.Application.Payments.QrCodeProviders;

public interface IQrCodeProvider
{
    string ProviderId { get; }

    Task<Result<QrCodeProviderResponse>> GenerateAsync(
        QrCodeRequest request,
        CancellationToken cancellationToken);
}

public record QrCodeProviderResponse(
    string EmvPayload,
    string QrCodeText);
