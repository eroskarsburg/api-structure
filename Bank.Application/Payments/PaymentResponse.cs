namespace Bank.Application.Payments;

public record PaymentResponse(Guid TransactionId, string EndToEndId, string Status, DateTime CreatedAtUtc);
