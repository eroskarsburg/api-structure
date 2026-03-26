using Bank.Application.Login;
using Bank.Application.Payments;
using Bank.Application.Payments.ProviderSelection;
using Bank.Application.Payments.QrCodeProviders;
using Bank.Infrastructure.ProviderSelection;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IQrCodeService, QrCodeService>();

builder.Services.AddSingleton<ICustomerAccountPaymentProviderConfigRepository, InMemoryCustomerAccountPaymentProviderConfigRepository>();
builder.Services.AddSingleton<IPaymentProviderPriorityResolver, PaymentProviderPriorityResolver>();

builder.Services.AddSingleton<IQrCodeProvider, PixEmvStaticQrCodeProvider>();
builder.Services.AddSingleton<IQrCodeProvider, FailingQrCodeProvider>();
builder.Services.AddSingleton<IQrCodeProviderCatalog, QrCodeProviderCatalog>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
