using Bank.Application.Payments.v1;
using Bank.Shared.Result;

namespace Bank.Application.Payments.Mediator;

public class PaymentRegistrationComponent : PaymentBaseComponent
{
    public void Register(PaymentContext context)
    {
        var response = new PaymentResponse(
            Guid.NewGuid(),
            $"E2E{DateTime.UtcNow:yyyyMMddHHmmssfff}",
            "Created",
            DateTime.UtcNow);

        context.Result = Result<PaymentResponse>.Success(response);
        Mediator?.Notify(this, "payment:registered", context);
    }
}
