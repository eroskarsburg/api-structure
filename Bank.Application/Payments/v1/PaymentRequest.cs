using Bank.Shared.Result;

namespace Bank.Application.Payments.v1;

public record PaymentRequest(decimal Amount, string PixKey, string PayerName, string PayerTaxId);
