namespace Bank.Application.QrCode.v1;

public record QrCodeResponse(
    Guid QrCodeId,
    string EmvPayload,
    string QrCodeText,
    DateTime CreatedAtUtc,
    DateTime? ExpiresAtUtc);
