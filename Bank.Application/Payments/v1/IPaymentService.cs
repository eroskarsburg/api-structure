using Bank.Shared.Result;

namespace Bank.Application.Payments.v1;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> ProcessAsync(PaymentRequest request, CancellationToken cancellationToken);
}
