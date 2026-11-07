using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Models;

namespace CloudSyncPulse.Tests.UnitTests
{
    [TestFixture]
    public class SyncRulesTests
    {
        [Test]
        public void Create_ValidMinimalData_ReturnsSuccess()
        {
            var providerId = Guid.NewGuid();
            var result = SyncRules.Create("Правило", "/tmp", LocalPathType.File, providerId, "0 0 * * *");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value!.Name, Is.EqualTo("Правило"));
            Assert.That(result.Value.LocalPath, Is.EqualTo("/tmp"));
            Assert.That(result.Value.LocalPathType, Is.EqualTo(LocalPathType.File));
            Assert.That(result.Value.ProviderId, Is.EqualTo(providerId));
            Assert.That(result.Value.Cron, Is.EqualTo("0 0 * * *"));
            Assert.That(result.Value.Enabled, Is.True);
            Assert.That(result.Value.Recursive, Is.False);
            Assert.That(result.Value.RemoteFolder, Is.EqualTo("/"));
            Assert.That(result.Value.OverwritePolicy, Is.EqualTo(OverwritePolicy.Skip));
            Assert.That(result.Value.DeleteAfterUpload, Is.False);
            Assert.That(result.Value.IsDeleted, Is.False);
            Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Create_WithAllOptionalFields_ReturnsSuccess()
        {
            var providerId = Guid.NewGuid();
            var result = SyncRules.Create(
                "Правило", "/tmp", LocalPathType.Directory, providerId, "*/5 * * * *",
                remoteFolder: "/backups",
                enabled: false,
                recursive: true,
                remoteFileNameTemplate: "{name}_{yyyyMMdd}",
                overwritePolicy: OverwritePolicy.Overwrite,
                deleteAfterUpload: true);

            Assert.That(result.IsSuccess, Is.True);
            var v = result.Value!;
            Assert.That(v.RemoteFolder, Is.EqualTo("/backups"));
            Assert.That(v.Enabled, Is.False);
            Assert.That(v.Recursive, Is.True);
            Assert.That(v.RemoteFileNameTemplate, Is.EqualTo("{name}_{yyyyMMdd}"));
            Assert.That(v.OverwritePolicy, Is.EqualTo(OverwritePolicy.Overwrite));
            Assert.That(v.DeleteAfterUpload, Is.True);
        }

        [Test]
        public void Create_TrimsName()
        {
            var result = SyncRules.Create("  Правило  ", "/tmp", LocalPathType.File,
                Guid.NewGuid(), "0 0 * * *");

            Assert.That(result.Value!.Name, Is.EqualTo("Правило"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyRemoteFolder_DefaultsToRoot(string? folder)
        {
            var result = SyncRules.Create("Правило", "/tmp", LocalPathType.File,
                Guid.NewGuid(), "0 0 * * *", remoteFolder: folder!);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value!.RemoteFolder, Is.EqualTo("/"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyName_ReturnsFail(string? name)
        {
            var result = SyncRules.Create(name!, "/tmp", LocalPathType.File,
                Guid.NewGuid(), "0 0 * * *");

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }

        [Test]
        public void Create_NameTooLong_ReturnsFail()
        {
            var name = new string('a', 201);
            var result = SyncRules.Create(name, "/tmp", LocalPathType.File,
                Guid.NewGuid(), "0 0 * * *");

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("200"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyLocalPath_ReturnsFail(string? path)
        {
            var result = SyncRules.Create("Правило", path!, LocalPathType.File,
                Guid.NewGuid(), "0 0 * * *");

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Create_InvalidLocalPathType_ReturnsFail()
        {
            var result = SyncRules.Create("Правило", "/tmp", (LocalPathType)999,
                Guid.NewGuid(), "0 0 * * *");

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Create_EmptyProviderId_ReturnsFail()
        {
            var result = SyncRules.Create("Правило", "/tmp", LocalPathType.File,
                Guid.Empty, "0 0 * * *");

            Assert.That(result.IsSuccess, Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyCron_ReturnsFail(string? cron)
        {
            var result = SyncRules.Create("Правило", "/tmp", LocalPathType.File,
                Guid.NewGuid(), cron!);

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Create_InvalidOverwritePolicy_ReturnsFail()
        {
            var result = SyncRules.Create("Правило", "/tmp", LocalPathType.File,
                Guid.NewGuid(), "0 0 * * *", overwritePolicy: (OverwritePolicy)999);

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Restore_CopiesAllFields()
        {
            var id = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var last = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var next = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);

            var r = SyncRules.Restore(
                id, "Правило", false, "/tmp", LocalPathType.Directory, true, providerId,
                "/backups", "{name}", OverwritePolicy.Rename, true, "*/10 * * * *",
                last, next, true);

            Assert.That(r.Id, Is.EqualTo(id));
            Assert.That(r.Name, Is.EqualTo("Правило"));
            Assert.That(r.Enabled, Is.False);
            Assert.That(r.LocalPath, Is.EqualTo("/tmp"));
            Assert.That(r.LocalPathType, Is.EqualTo(LocalPathType.Directory));
            Assert.That(r.Recursive, Is.True);
            Assert.That(r.ProviderId, Is.EqualTo(providerId));
            Assert.That(r.RemoteFolder, Is.EqualTo("/backups"));
            Assert.That(r.RemoteFileNameTemplate, Is.EqualTo("{name}"));
            Assert.That(r.OverwritePolicy, Is.EqualTo(OverwritePolicy.Rename));
            Assert.That(r.DeleteAfterUpload, Is.True);
            Assert.That(r.Cron, Is.EqualTo("*/10 * * * *"));
            Assert.That(r.LastRunUtc, Is.EqualTo(last));
            Assert.That(r.NextRunUtc, Is.EqualTo(next));
            Assert.That(r.IsDeleted, Is.True);
        }

        [Test]
        public void Restore_NullStrings_BecomeDefaults()
        {
            var r = SyncRules.Restore(Guid.NewGuid(), null!, true, null!, LocalPathType.File,
                false, Guid.NewGuid(), null!, null, OverwritePolicy.Skip, false, null!,
                null, null, false);

            Assert.That(r.Name, Is.Empty);
            Assert.That(r.LocalPath, Is.Empty);
            Assert.That(r.RemoteFolder, Is.EqualTo("/"));
            Assert.That(r.Cron, Is.EqualTo("0 0 * * *"));
        }
    }
}
