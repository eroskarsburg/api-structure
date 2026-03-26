namespace Bank.Application.Payments;

public record QrCodeGenerationRequest(
    Guid CustomerAccountId,
    decimal Amount,
    string PixKey,
    string PayerName,
    string Description,
    DateTime? ExpiresAtUtc);
