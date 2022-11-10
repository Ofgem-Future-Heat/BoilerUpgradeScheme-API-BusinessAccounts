using Microsoft.EntityFrameworkCore;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Domain.Interfaces;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;
using Ofgem.Lib.BUS.AuditLogging.Domain.Entities;
using Ofgem.Lib.BUS.AuditLogging.Interfaces;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;

public class BusinessAccountsDbContext : DbContext, IAuditLogsDbContext
{
    public BusinessAccountsDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BusinessAccount> BusinessAccounts { get; set; }
    public DbSet<BusinessDashboard> BusinessDashboards { get; set; }
    public DbSet<BusinessAddress> BusinessAddresses { get; set; }
    public DbSet<BankAccount> BankAccounts{ get; set; }
    public DbSet<BankAccountStatus> BankAccountStatuses { get; set; }
    public DbSet<CompanyType> CompanyTypes { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<BusinessAccountStatus> BusinessAccountStatuses { get; set; }
    public DbSet<BusinessAccountSubStatus> BusinessAccountSubStatuses{ get; set; }
    public DbSet<ExternalUserAccount> ExternalUserAccounts { get; set; }
    public DbSet<CompaniesHouseDetail> CompaniesHouseDetails { get; set; }
    public DbSet<GlobalSetting> GlobalSettings { get; set; }
    public DbSet<CertificationDetail> CertificationDetails { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<BusinessAccountStatusHistory> BusinessAccountStatusHistories { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<InviteStatus> InviteStatuses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BusinessAccountEntityTypeConfiguration).Assembly);

        modelBuilder.Entity<GlobalSetting>().Property(b => b.ID).HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<BusinessDashboard>(eb => { eb.HasNoKey(); eb.ToView("vw_Dashboard_Business_Only"); });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var AddedEntities = ChangeTracker.Entries<ICreateModify>()
            .Where(entity => entity.State == EntityState.Added)
            .ToList();

        AddedEntities.ForEach(entity =>
        {
            entity.Property(x => x.CreatedDate).CurrentValue = DateTime.UtcNow;
            entity.Property(x => x.CreatedDate).IsModified = true;

        });

        var EditedEntities = ChangeTracker.Entries<ICreateModify>()
            .Where(entity => entity.State == EntityState.Modified)
            .ToList();

        EditedEntities.ForEach(entity =>
        {
            entity.Property(x => x.LastUpdatedDate).CurrentValue = DateTime.UtcNow;
            entity.Property(x => x.LastUpdatedDate).IsModified = true;

            entity.Property(x => x.CreatedDate).CurrentValue = entity.Property(x => x.CreatedDate).OriginalValue;
            entity.Property(x => x.CreatedDate).IsModified = false;
        });

        return base.SaveChangesAsync(true, cancellationToken);
    }
}
