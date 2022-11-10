
# Please note
- Entities (models for the database) in are in the Domain Class Lib /Entities.
- To Configure database migrations/ schema please use the (Fluent API)[https://docs.microsoft.com/en-us/ef/core/modeling/#use-fluent-api-to-configure-a-model] and only uses the Configuration classes in Providers.DataAccess/Configurations.


## Key Resources

- [EF Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core - Creating and configuring a model](https://docs.microsoft.com/en-us/ef/core/modeling/)
- [Using EF Core in a Separate Class Library](https://dotnetthoughts.net/using-ef-core-in-a-separate-class-library/)

## Migration Commands

Adding a migration to solution (from root):

```
dotnet ef migrations add {{Migration Name}} --project .\src\Providers\Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess\Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.csproj --startup-project .\src\Hosting\Ofgem.API.BUS.BusinessAccounts.WebApp\Ofgem.API.BUS.BusinessAccounts.WebApp.csproj
```

Remove migration from solution:

```
dotnet ef migrations remove --project .\src\Providers\Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess\Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.csproj --startup-project .\src\Hosting\Ofgem.API.BUS.BusinessAccounts.WebApp\Ofgem.API.BUS.BusinessAccounts.WebApp.csproj
```

Update database with new migration scripts

```
dotnet ef database update --project .\src\Providers\Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess\Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.csproj --startup-project .\src\Hosting\Ofgem.API.BUS.BusinessAccounts.WebApp\Ofgem.API.BUS.BusinessAccounts.WebApp.csproj

```