using CloudSyncPulse.DataAccess.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudSyncPulse.DataAccess.Sqlite.Configurations
{
    public class SyncRulesEntityConfiguration : IEntityTypeConfiguration<SyncRulesEntity>
    {
        public void Configure(EntityTypeBuilder<SyncRulesEntity> builder)
        {
            builder.ToTable("SyncRules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Enabled)
                   .IsRequired();

            builder.Property(x => x.LocalPath)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(x => x.LocalPathType)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.Recursive)
                   .IsRequired();

            builder.Property(x => x.ProviderId)
                   .IsRequired();

            builder.Property(x => x.RemoteFolder)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(x => x.RemoteFileNameTemplate)
                   .HasMaxLength(300);

            builder.Property(x => x.OverwritePolicy)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.DeleteAfterUpload)
                   .IsRequired();

            builder.Property(x => x.Cron)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            builder.HasIndex(x => new { x.Enabled, x.IsDeleted, x.NextRunUtc });

            builder.HasOne(x => x.Provider)
                   .WithMany(p => p.SyncRules)
                   .HasForeignKey(x => x.ProviderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Runs)
                   .WithOne(r => r.SyncRule)
                   .HasForeignKey(r => r.SyncRuleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
