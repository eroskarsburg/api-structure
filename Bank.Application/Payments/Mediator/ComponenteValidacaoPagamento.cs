using Bank.Shared.Errors;
using Bank.Shared.Result;

namespace Bank.Application.Payments.Mediator;

public class PaymentValidationComponent : PaymentBaseComponent
{
    public void Start(PaymentContext context)
    {
        var errors = new List<Error>();

        if (context.Request.Amount <= 0)
        {
            errors.Add(Error.Create("VALIDATION_ERROR", "Amount must be greater than zero.", "amount"));
        }

        if (string.IsNullOrWhiteSpace(context.Request.PixKey))
        {
            errors.Add(Error.Create("VALIDATION_ERROR", "PixKey is required.", "pixKey"));
        }

        if (errors.Count > 0)
        {
            context.Result = Result<PaymentResponse>.BadRequest(errors);
            Mediator?.Notify(this, "payment:validation_failed", context);
            return;
        }

        Mediator?.Notify(this, "payment:validated", context);
    }
}
