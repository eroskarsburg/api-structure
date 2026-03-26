using Bank.Shared.Result;

namespace Bank.Application.Payments;

public interface IQrCodeService
{
    Task<Result<QrCodeGenerationResponse>> GenerateAsync(QrCodeGenerationRequest request, CancellationToken cancellationToken);
    Task<Result<QrCodeGenerationResponse>> GetByIdAsync(Guid qrCodeId, CancellationToken cancellationToken);
}
