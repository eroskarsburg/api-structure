namespace Bank.Application.Payments.ProviderSelection;

public interface ICustomerAccountPaymentProviderConfigRepository
{
    Task<IReadOnlyList<CustomerAccountPaymentProviderConfig>> ListByCustomerAccountIdAsync(
        Guid customerAccountId,
        CancellationToken cancellationToken);
}
