using CloudSyncPulse.DataAccess.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudSyncPulse.DataAccess.Sqlite.Configurations
{
    public class ProvidersEntityConfiguration : IEntityTypeConfiguration<ProvidersEntity>
    {
        public void Configure(EntityTypeBuilder<ProvidersEntity> builder)
        {
            builder.ToTable("Providers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.DisplayName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.IsEnabled)
                   .IsRequired();

            builder.Property(x => x.CreatedUtc)
                   .IsRequired();

            builder.Property(x => x.UpdatedUtc)
                   .IsRequired();

            builder.HasIndex(x => new { x.Type, x.DisplayName })
                   .IsUnique();

            builder.HasOne(x => x.Credential)
                   .WithOne(c => c.Provider)
                   .HasForeignKey<ProviderCredentialEntity>(c => c.ProviderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SyncRules)
                   .WithOne(r => r.Provider)
                   .HasForeignKey(r => r.ProviderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
