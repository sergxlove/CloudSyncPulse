using CloudSyncPulse.DataAccess.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudSyncPulse.DataAccess.Sqlite.Configurations
{
    public class SyncRunEntityConfiguration : IEntityTypeConfiguration<SyncRunEntity>
    {
        public void Configure(EntityTypeBuilder<SyncRunEntity> builder)
        {
            builder.ToTable("SyncRuns");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SyncRuleId)
                   .IsRequired();

            builder.Property(x => x.StartedUtc)
                   .IsRequired();

            builder.Property(x => x.FinishedUtc);

            builder.Property(x => x.Status)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.Trigger)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(x => x.FilesTotal).IsRequired();
            builder.Property(x => x.FilesSucceeded).IsRequired();
            builder.Property(x => x.FilesFailed).IsRequired();
            builder.Property(x => x.BytesUploaded).IsRequired();

            builder.Property(x => x.ErrorMessage)
                   .HasMaxLength(4000);

            builder.HasIndex(x => new { x.SyncRuleId, x.StartedUtc });

            builder.HasOne(x => x.SyncRule)
                   .WithMany(r => r.Runs)
                   .HasForeignKey(x => x.SyncRuleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
