namespace Bank.Application.Payments.v1;

public record PaymentResponse(Guid TransactionId, string EndToEndId, string Status, DateTime CreatedAtUtc);
