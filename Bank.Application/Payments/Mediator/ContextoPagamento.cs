using Bank.Shared.Result;

namespace Bank.Application.Payments.Mediator;

public class PaymentContext
{
    public PaymentContext(PaymentRequest request)
    {
        Request = request;
    }

    public PaymentRequest Request { get; }
    public Result<PaymentResponse>? Result { get; set; }
}
