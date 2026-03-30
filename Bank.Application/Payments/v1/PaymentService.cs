using Bank.Application.Payments.Mediator;
using Bank.Shared.Result;

namespace Bank.Application.Payments.v1;

public class PaymentService : IPaymentService
{
    public Task<Result<PaymentResponse>> ProcessAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        var context = new PaymentContext(request);
        var mediator = new PaymentMediator(
            new PaymentValidationComponent(),
            new PaymentRegistrationComponent(),
            new PaymentNotificationComponent());

        mediator.Start(context);

        return Task.FromResult(context.Result ?? Result<PaymentResponse>.Fail(Bank.Shared.Errors.Error.Create("UNEXPECTED_ERROR", "Unable to process payment.")));
    }

}
