namespace Bank.Application.Payments.Mediator;

public abstract class PaymentBaseComponent
{
    protected IPaymentMediator? Mediator { get; private set; }

    public void SetMediator(IPaymentMediator mediator)
    {
        Mediator = mediator;
    }
}
