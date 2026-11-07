using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Models;

namespace CloudSyncPulse.Tests.UnitTests
{
    [TestFixture]
    public class ProvidersTests
    {

        [Test]
        public void Create_ValidData_ReturnsSuccess()
        {
            var result = Providers.Create(ProviderType.GoogleDrive, "Мой Google");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value!.Type, Is.EqualTo(ProviderType.GoogleDrive));
            Assert.That(result.Value.DisplayName, Is.EqualTo("Мой Google"));
            Assert.That(result.Value.IsEnabled, Is.True);
            Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Create_SetsCreatedAndUpdatedUtcToNow()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var result = Providers.Create(ProviderType.Dropbox, "Dropbox");
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.That(result.Value!.CreatedUtc, Is.InRange(before, after));
            Assert.That(result.Value.UpdatedUtc, Is.EqualTo(result.Value.CreatedUtc));
        }

        [Test]
        public void Create_TrimsDisplayName()
        {
            var result = Providers.Create(ProviderType.YandexDisk, "  Яндекс  ");

            Assert.That(result.Value!.DisplayName, Is.EqualTo("Яндекс"));
        }

        [Test]
        public void Create_DisabledFlag()
        {
            var result = Providers.Create(ProviderType.GoogleDrive, "Google", isEnabled: false);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value!.IsEnabled, Is.False);
        }


        [Test]
        public void Create_InvalidEnum_ReturnsFail()
        {
            var result = Providers.Create((ProviderType)999, "X");

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyDisplayName_ReturnsFail(string? name)
        {
            var result = Providers.Create(ProviderType.GoogleDrive, name!);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }

        [Test]
        public void Create_DisplayNameTooLong_ReturnsFail()
        {
            var name = new string('a', 201);
            var result = Providers.Create(ProviderType.GoogleDrive, name);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("200"));
        }

        [Test]
        public void Create_DisplayNameExactlyMaxLength_ReturnsSuccess()
        {
            var name = new string('a', 200);
            var result = Providers.Create(ProviderType.GoogleDrive, name);

            Assert.That(result.IsSuccess, Is.True);
        }

        // ---------- Restore ----------

        [Test]
        public void Restore_CopiesAllFields()
        {
            var id = Guid.NewGuid();
            var created = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc);
            var updated = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc);

            var p = Providers.Restore(id, ProviderType.YandexDisk, "Яндекс", false, created, updated);

            Assert.That(p.Id, Is.EqualTo(id));
            Assert.That(p.Type, Is.EqualTo(ProviderType.YandexDisk));
            Assert.That(p.DisplayName, Is.EqualTo("Яндекс"));
            Assert.That(p.IsEnabled, Is.False);
            Assert.That(p.CreatedUtc, Is.EqualTo(created));
            Assert.That(p.UpdatedUtc, Is.EqualTo(updated));
        }

        [Test]
        public void Restore_NullDisplayName_BecomesEmpty()
        {
            var p = Providers.Restore(Guid.NewGuid(), ProviderType.GoogleDrive, null!,
                true, DateTime.UtcNow, DateTime.UtcNow);

            Assert.That(p.DisplayName, Is.Empty);
        }
    }
}
