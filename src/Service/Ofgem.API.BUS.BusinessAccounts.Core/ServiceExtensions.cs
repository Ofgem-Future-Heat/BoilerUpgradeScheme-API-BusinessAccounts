using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notify.Client;
using Notify.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Core.Automapper.Profiles;
using Ofgem.API.BUS.BusinessAccounts.Core.FluentValidation;
using Ofgem.API.BUS.BusinessAccounts.Core.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Core;

/// <summary>
/// Service Extensions to be added to program.cs
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Configuration to add Services to WebApi.
    /// </summary>
    /// <param name="services">IServiceCollection to build the service at start up.</param>
    /// <param name="config">IConfiguration for configuring the service.</param>
    /// <returns>IServiceCollection of a transient service when starting up the generic host.</returns>
    public static IServiceCollection AddServiceConfigurations(this IServiceCollection services, IConfiguration config)
    {
        var govNotifyConfig = config.GetSection("GovNotify");
        string apiKey = govNotifyConfig["APIKey"];
        string inviteEmailTemplateId = govNotifyConfig["InviteEmailTemplateId"];
        string inviteTokenSecret = govNotifyConfig["InviteTokenSecret"];
        string externalPortalURL = govNotifyConfig["externalPortalUrl"];
        string installerReplyToAddress = govNotifyConfig["InstallerReplyToAddress"];

        services.AddTransient<IAsyncNotificationClient>(s => new NotificationClient(apiKey));
        services.AddTransient<IAccountsService,BusinessAccountsService>();
        services.AddTransient<IInviteService, InviteService>();
        services.AddTransient<IInviteTokenServiceOptions>(s => new InviteTokenServiceOptions(inviteEmailTemplateId, inviteTokenSecret, externalPortalURL, installerReplyToAddress));
        AddAutoMapperConfiguration(services,config);
        AddFluentValidationConfiguration(services,config);
            
        return services; 
    }

    /// <summary>
    /// Adds the service configuration for Automapper.
    /// </summary>
    /// <param name="services">IServiceCollection to build the service at start up.</param>
    /// <param name="config">IConfiguration for configuring the service.</param>
    /// <returns>IServiceCollection of a transient service when starting up the generic host.</returns>
    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var type = typeof(PostBusinessAccountRequestProfiler);
        var assemblyFromType = type.Assembly;
        services.AddAutoMapper(assemblyFromType);

        return services;
    }

    /// <summary>
    /// Adds the configuration for Fluent validation
    /// </summary>
    /// <param name="services">IServiceCollection to build the service at start up.</param>
    /// <param name="config">IConfiguration for configuring the service.</param>
    /// <returns>IServiceCollection of a transient service when starting up the generic host.</returns>
    public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddFluentValidation(fv =>
        {
            fv.DisableDataAnnotationsValidation = true;
            fv.RegisterValidatorsFromAssemblyContaining<PostBusinessAccountRequestValidator>();
        });

        return services;
    }
}