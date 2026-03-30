using Bank.API.DependencyInjection;
using Bank.API.Routing;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseRouteTokenTransformer()));
});
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Bank API", Version = "v2" });
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        var path = apiDesc.RelativePath ?? "";
        return docName switch
        {
            "v1" => path.StartsWith("v1/", StringComparison.OrdinalIgnoreCase),
            "v2" => path.StartsWith("v2/", StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Bank API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
