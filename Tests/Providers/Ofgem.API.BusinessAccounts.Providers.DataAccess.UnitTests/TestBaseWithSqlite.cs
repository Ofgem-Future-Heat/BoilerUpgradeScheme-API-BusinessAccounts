using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Ofgem.API.BUS.BusinessAccounts.Domain.Constants;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess
{
    public class TestBusinessAccountsDbContext : BusinessAccountsDbContext
    {
        public TestBusinessAccountsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BusinessAccountEntityTypeConfiguration).Assembly);
            modelBuilder.Entity<BusinessDashboard>().HasKey(nameof(BusinessDashboard.BusinessID), nameof(BusinessDashboard.BusinessSubStatusCode));
        }
    }

    /// <summary>
    /// This Class forms as a provider to unit tests (or any other test suite), to instanciate a mock Business Accounts Db Context.
    /// </summary>
    public class TestBaseWithSqlite
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private SqliteConnection _connection;
        public TestBusinessAccountsDbContext DbContext;

        public TestBaseWithSqlite()
        {
            
        }

        [SetUp]
        public void Setup()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();

            var options = new DbContextOptionsBuilder<BusinessAccountsDbContext>()
                .UseSqlite(_connection)
                .Options;

            DbContext = new TestBusinessAccountsDbContext(options);
            DbContext.Database.EnsureCreated();

            SeedInMemoryDb();
        }

        /// <summary>
        /// This function seeds the mock database.
        /// </summary>
        private void SeedInMemoryDb()
        {
            var addressTypes = new List<AddressType>
            {
                new AddressType
                {
                    Id = new Guid("2B7C0FBE-81FB-48AF-B3D4-99B6941C3CB0"),
                    Description = "Business Address type desc",
                    DisplayName = "Business Address",
                    Code = AddressType.AddressTypeCode.BIZ,
                    SortOrder = 10
                },
                new AddressType
                {
                    Id = new Guid("454ACCB1-3D67-4FCC-A92E-DCB07384CA14"),
                    Description = "Trading Address type desc",
                    DisplayName = "Trading Address",
                    Code = AddressType.AddressTypeCode.TRADE,
                    SortOrder = 20
                }
            };
            DbContext.AddRange(addressTypes);

            

            var companyType = new CompanyType
            {
                Id = Guid.NewGuid(),
                Description = "Energy Company"
            };
            DbContext.Add(companyType);

            var status = new BusinessAccountStatus
            {
                Id = Guid.NewGuid(),
                Description = "Active",
                DisplayName = "Active",
                Code = BusinessAccountStatus.BusinessAccountStatusCode.ACTIVE,
                SortOrder = 10
            };
            DbContext.Add(status);

            var subStatus = new BusinessAccountSubStatus
            {
                Id = Guid.NewGuid(),
                Description = "Active",
                DisplayName = "Active",
                Code = BusinessAccountSubStatus.BusinessAccountSubStatusCode.ACTIV,
                BusinessAccountStatusId = status.Id,
                SortOrder = 10
            };
            DbContext.Add(subStatus);

            var bankAccountStatus = new BankAccountStatus
            {
                Id = new Guid("9c328895-0023-4776-a0eb-7ad73ee51caa"),
                Code = BankAccountStatus.BankAccountStatusCode.ACTIVE,
                Description = "Test Status Description",
                DisplayName = "Test Status Display"
            };
            DbContext.BankAccountStatuses.Add(bankAccountStatus);

            var businessAccount = new BusinessAccount
            {
                Id = Guid.NewGuid(),
                SubStatusId = subStatus.Id,
                CompanyTypeId = companyType.Id,
                BusinessAccountNumber = "123",
                BusinessName = "Taylor Wimpey",
                TradingName = "Taylor WImpey Limited",
                MCSCertificationNumber = "1253586792",
                MCSCertificationBody = "Certification Body",
                MCSMembershipNumber = "MCS128373",
                MCSConsumerCode = "iwejfjiow",
                MCSCompanyType = "4",
                MCSId = "71234h",
                MCSAddressID = Guid.NewGuid(),
                CreatedBy = "Tests",
                BankAccounts = new() 
                { 
                    new BankAccount() 
                    { 
                        AccountName = "TestBankAccount",
                        SortCode = "000000",
                        AccountNumber = "00000000",
                        Status = bankAccountStatus,
                        CreatedBy = "Tests"
                    } }
            };
            DbContext.Add(businessAccount);

            var address = new BusinessAddress
            {
                Id = Guid.NewGuid(),
                UPRN = "100023336956",
                AddressLine1 = "10 Downing Street",
                County = "Westminster",
                Postcode = "SW1A",
                CreatedBy = "Tests",
                AddressTypeId = TypeMappings.AddressType[AddressType.AddressTypeCode.BIZ].Id,
                BusinessAccountId = businessAccount.Id
            };
            DbContext.Add(address);

            var externalUser = new ExternalUserAccount
            {
                FirstName = "Test",
                LastName = "Testerson",
                EmailAddress = "test.testerson@ofgem.gov.uk",
                TelephoneNumber = "01234 567890",
                AuthorisedRepresentative = true,
                SuperUser = true,
                StandardUser = true,
                BusinessAccountID = businessAccount.Id,
                HomeAddress = "10 Downing Street, London",
                CreatedBy = "Tests"
            };
            DbContext.ExternalUserAccounts.Add(externalUser);

            var inviteStatus = new InviteStatus
            {
                Id = Guid.NewGuid(),
                Description = "Test Status Description",
                DisplayName = "Test Status Display"
            };
            DbContext.InviteStatuses.Add(inviteStatus);

            DbContext.SaveChanges();
        }

        /// <summary>
        /// This function deletes the mock database after its use is not needed.
        /// Its should be implemented at the end of every test run.
        /// </summary>
        public void Dispose()
        {
            DbContext.Dispose();    
        }
    }
}
