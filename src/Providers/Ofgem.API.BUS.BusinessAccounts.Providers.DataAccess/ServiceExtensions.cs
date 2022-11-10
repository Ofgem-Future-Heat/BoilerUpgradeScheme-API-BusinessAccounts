using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Interfaces;
using Ofgem.Lib.BUS.AuditLogging.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;

/// <summary>
/// Service Extensions to be added to program.cs
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configuration to add BusinessAccountsDbContext to WebApi
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddBusinessAccountsDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BusinessAccountsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BusinessAccountsConnection"),
            assembly => assembly.MigrationsAssembly(typeof(BusinessAccountsDbContext).Assembly.FullName)));

        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddScoped<IAuditLogsDbContext, BusinessAccountsDbContext>();

        services.AddTransient<IBusinessAccountProvider,BusinessAccountProvider>();

        return services;
    }
}
