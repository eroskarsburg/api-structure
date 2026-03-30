using Bank.Application.Login;
using Bank.Application.Payments.ProviderSelection;
using Bank.Application.Payments.QrCodeProviders;
using Bank.Application.Payments.v1;
using Bank.Application.QrCode.v1;
using Bank.Infrastructure.ProviderSelection;

namespace Bank.API.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddSingleton<IQrCodeService, QrCodeService>();

        services.AddSingleton<ICustomerAccountPaymentProviderConfigRepository, InMemoryCustomerAccountPaymentProviderConfigRepository>();
        services.AddSingleton<IPaymentProviderPriorityResolver, PaymentProviderPriorityResolver>();

        services.AddSingleton<IQrCodeProvider, PixEmvStaticQrCodeProvider>();
        services.AddSingleton<IQrCodeProvider, FailingQrCodeProvider>();
        services.AddSingleton<IQrCodeProviderCatalog, QrCodeProviderCatalog>();

        return services;
    }
}
