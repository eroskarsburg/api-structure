namespace Bank.Application.QrCode.v1;

public record QrCodeRequest(
    Guid CustomerAccountId,
    decimal Amount,
    string PixKey,
    string PayerName,
    string Description,
    DateTime? ExpiresAtUtc);
