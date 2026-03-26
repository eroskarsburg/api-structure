namespace Bank.Application.Payments;

public record QrCodeGenerationResponse(
    Guid QrCodeId,
    string EmvPayload,
    string QrCodeText,
    DateTime CreatedAtUtc,
    DateTime? ExpiresAtUtc);
