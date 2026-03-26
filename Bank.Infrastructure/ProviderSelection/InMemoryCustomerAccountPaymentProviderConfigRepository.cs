using Bank.Application.Payments.ProviderSelection;

namespace Bank.Infrastructure.ProviderSelection;

public sealed class InMemoryCustomerAccountPaymentProviderConfigRepository : ICustomerAccountPaymentProviderConfigRepository
{
    private readonly IReadOnlyList<CustomerAccountPaymentProviderConfig> _configs;

    public InMemoryCustomerAccountPaymentProviderConfigRepository()
    {
        var sampleCustomerAccountId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        _configs =
        [
            new CustomerAccountPaymentProviderConfig(sampleCustomerAccountId, "pix_emv_static", Priority: 1, Enabled: true),
            new CustomerAccountPaymentProviderConfig(sampleCustomerAccountId, "always_fail", Priority: 2, Enabled: true)
        ];
    }

    public Task<IReadOnlyList<CustomerAccountPaymentProviderConfig>> ListByCustomerAccountIdAsync(
        Guid customerAccountId,
        CancellationToken cancellationToken)
    {
        var result = _configs
            .Where(c => c.CustomerAccountId == customerAccountId)
            .ToArray();

        return Task.FromResult<IReadOnlyList<CustomerAccountPaymentProviderConfig>>(result);
    }
}

