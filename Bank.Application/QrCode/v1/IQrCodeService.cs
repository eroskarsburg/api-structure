using Bank.Shared.Result;

namespace Bank.Application.QrCode.v1;

public interface IQrCodeService
{
    Task<Result<QrCodeResponse>> GenerateAsync(QrCodeRequest request, CancellationToken cancellationToken);
    Task<Result<QrCodeResponse>> GetByIdAsync(Guid qrCodeId, CancellationToken cancellationToken);
}
