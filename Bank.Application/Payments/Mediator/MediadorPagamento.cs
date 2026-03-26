namespace Bank.Application.Payments.Mediator;

public class PaymentMediator : IPaymentMediator
{
    private readonly PaymentValidationComponent _validationComponent;
    private readonly PaymentRegistrationComponent _registrationComponent;
    private readonly PaymentNotificationComponent _notificationComponent;

    public PaymentMediator(
        PaymentValidationComponent validationComponent,
        PaymentRegistrationComponent registrationComponent,
        PaymentNotificationComponent notificationComponent)
    {
        _validationComponent = validationComponent;
        _registrationComponent = registrationComponent;
        _notificationComponent = notificationComponent;

        _validationComponent.SetMediator(this);
        _registrationComponent.SetMediator(this);
        _notificationComponent.SetMediator(this);
    }

    public void Start(PaymentContext context)
    {
        _validationComponent.Start(context);
    }

    public void Notify(object sender, string eventName, PaymentContext context)
    {
        if (eventName == "payment:validated")
        {
            _registrationComponent.Register(context);
        }
        else if (eventName == "payment:registered")
        {
            _notificationComponent.Publish(context);
        }
    }
}
