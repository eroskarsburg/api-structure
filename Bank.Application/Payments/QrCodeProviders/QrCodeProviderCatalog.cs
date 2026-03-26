namespace Bank.Application.Payments.QrCodeProviders;

public interface IQrCodeProviderCatalog
{
    bool TryGet(string providerId, out IQrCodeProvider provider);
}

public sealed class QrCodeProviderCatalog : IQrCodeProviderCatalog
{
    private readonly IReadOnlyDictionary<string, IQrCodeProvider> _map;

    public QrCodeProviderCatalog(IEnumerable<IQrCodeProvider> providers)
    {
        _map = providers.ToDictionary(p => p.ProviderId, StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGet(string providerId, out IQrCodeProvider provider)
    {
        return _map.TryGetValue(providerId, out provider!);
    }
}

