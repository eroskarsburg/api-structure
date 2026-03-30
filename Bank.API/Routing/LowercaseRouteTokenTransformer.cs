namespace Bank.API.Routing;

public sealed class LowercaseRouteTokenTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is not string s || string.IsNullOrEmpty(s))
        {
            return null;
        }

        return s.ToLowerInvariant();
    }
}
