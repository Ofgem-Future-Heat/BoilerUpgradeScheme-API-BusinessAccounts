using Azure.Identity;
using Ofgem.API.BUS.BusinessAccounts.Core;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;
using Ofgem.Lib.BUS.AuditLogging.Extensions;
using Ofgem.Lib.BUS.Logging;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Data.SqlClient;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Azure Key Vault configuration
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddOfgemCloudApplicationInsightsTelemetry();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddBusinessAccountsDbContext(builder.Configuration);
builder.Services.AddServiceConfigurations(builder.Configuration);
builder.Services.AddAuditLoggingServices();

builder.Services.AddMvc().AddApplicationPart(typeof(Ofgem.API.BUS.BusinessAccounts.Api.McsController).GetTypeInfo().Assembly);
builder.Services.AddMvc().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next(context);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseTelemetryMiddleware();
app.Run();




