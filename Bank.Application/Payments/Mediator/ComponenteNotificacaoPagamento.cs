namespace Bank.Application.Payments.Mediator;

public class PaymentNotificationComponent : PaymentBaseComponent
{
    public void Publish(PaymentContext context)
    {
        Mediator?.Notify(this, "payment:notified", context);
    }
}
