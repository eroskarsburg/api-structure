namespace Bank.Application.Payments.ProviderSelection;

public sealed class PaymentProviderPriorityResolver : IPaymentProviderPriorityResolver
{
    private readonly ICustomerAccountPaymentProviderConfigRepository _repository;

    public PaymentProviderPriorityResolver(ICustomerAccountPaymentProviderConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<string>> ResolvePrioritizedProvidersAsync(
        Guid customerAccountId,
        CancellationToken cancellationToken)
    {
        var configs = await _repository.ListByCustomerAccountIdAsync(customerAccountId, cancellationToken);

        return configs
            .Where(c => c.Enabled)
            .OrderBy(c => c.Priority)
            .Select(c => c.ProviderId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}

