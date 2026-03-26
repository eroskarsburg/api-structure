namespace Bank.Application.Payments.Mediator;

public interface IPaymentMediator
{
    void Notify(object sender, string eventName, PaymentContext context);
}
