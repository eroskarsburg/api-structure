namespace Bank.Application.Payments.ProviderSelection;

public interface IPaymentProviderPriorityResolver
{
    Task<IReadOnlyList<string>> ResolvePrioritizedProvidersAsync(
        Guid customerAccountId,
        CancellationToken cancellationToken);
}

