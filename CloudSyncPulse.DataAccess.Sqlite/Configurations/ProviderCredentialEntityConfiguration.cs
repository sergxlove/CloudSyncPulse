using CloudSyncPulse.DataAccess.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudSyncPulse.DataAccess.Sqlite.Configurations
{
    public class ProviderCredentialEntityConfiguration : IEntityTypeConfiguration<ProviderCredentialEntity>
    {
        public void Configure(EntityTypeBuilder<ProviderCredentialEntity> builder)
        {
            builder.ToTable("ProviderCredentials");

            builder.HasKey(x => x.ProviderId);

            builder.Property(x => x.AccessTokenEncrypted)
                   .IsRequired();

            builder.Property(x => x.RefreshTokenEncrypted);

            builder.Property(x => x.ExpiresAtUtc);

            builder.Property(x => x.Scope)
                   .HasMaxLength(1000);

            builder.Property(x => x.TokenType)
                   .HasMaxLength(50);

            builder.Property(x => x.UpdatedUtc)
                   .IsRequired();

            builder.HasOne(x => x.Provider)
                   .WithOne(p => p.Credential)
                   .HasForeignKey<ProviderCredentialEntity>(x => x.ProviderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
