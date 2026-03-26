using Bank.Shared.Result;

namespace Bank.Application.Payments;

public record PaymentRequest(decimal Amount, string PixKey, string PayerName, string PayerTaxId);
