using CloudSyncPulse.DataAccess.Sqlite.Configurations;
using CloudSyncPulse.DataAccess.Sqlite.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSyncPulse.DataAccess.Sqlite
{
    public class CloudSyncDbContext : DbContext
    {
        public CloudSyncDbContext(DbContextOptions<CloudSyncDbContext> options) 
            : base(options)
        {
            
        }

        public DbSet<ProviderCredentialEntity> ProviderCredentialsTable { get; set; }
        public DbSet<ProvidersEntity> ProvidersTable { get; set; }
        public DbSet<SyncRulesEntity> SyncRulesTable { get; set; }
        public DbSet<SyncRunEntity> SyncRunTable { get; set; }
        public DbSet<UsersEntity> UsersTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProviderCredentialEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProvidersEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SyncRulesEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SyncRunEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UsersEntityConfiguration());
        }
    }
}
