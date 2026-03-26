using Bank.Shared.Result;

namespace Bank.Application.Payments;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> ProcessAsync(PaymentRequest request, CancellationToken cancellationToken);
}
