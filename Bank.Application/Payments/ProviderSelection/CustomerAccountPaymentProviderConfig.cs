namespace Bank.Application.Payments.ProviderSelection;

public record CustomerAccountPaymentProviderConfig(
    Guid CustomerAccountId,
    string ProviderId,
    int Priority,
    bool Enabled);
