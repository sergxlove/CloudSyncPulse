using CloudSyncPulse.Core.Models;

namespace CloudSyncPulse.Tests.UnitTests
{
    [TestFixture]
    public class ProviderCredentialTests
    {
        private static byte[] SampleToken() => new byte[] { 1, 2, 3, 4 };

        [Test]
        public void Create_ValidMinimalData_ReturnsSuccess()
        {
            var providerId = Guid.NewGuid();
            var result = ProviderCredential.Create(providerId, SampleToken());

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value!.ProviderId, Is.EqualTo(providerId));
            Assert.That(result.Value.AccessTokenEncrypted, Is.EqualTo(SampleToken()));
            Assert.That(result.Value.RefreshTokenEncrypted, Is.Null);
            Assert.That(result.Value.ExpiresAtUtc, Is.Null);
            Assert.That(result.Value.Scope, Is.Null);
            Assert.That(result.Value.TokenType, Is.Null);
        }

        [Test]
        public void Create_WithAllOptionalFields_ReturnsSuccess()
        {
            var providerId = Guid.NewGuid();
            var expires = DateTime.UtcNow.AddHours(1);
            var refresh = new byte[] { 9, 8, 7 };

            var result = ProviderCredential.Create(
                providerId, SampleToken(), refresh, expires, "drive.file", "Bearer");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value!.RefreshTokenEncrypted, Is.EqualTo(refresh));
            Assert.That(result.Value.ExpiresAtUtc, Is.EqualTo(expires));
            Assert.That(result.Value.Scope, Is.EqualTo("drive.file"));
            Assert.That(result.Value.TokenType, Is.EqualTo("Bearer"));
        }

        [Test]
        public void Create_SetsUpdatedUtc()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var result = ProviderCredential.Create(Guid.NewGuid(), SampleToken());
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.That(result.Value!.UpdatedUtc, Is.InRange(before, after));
        }

        [Test]
        public void Create_EmptyProviderId_ReturnsFail()
        {
            var result = ProviderCredential.Create(Guid.Empty, SampleToken());

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }

        [Test]
        public void Create_NullAccessToken_ReturnsFail()
        {
            var result = ProviderCredential.Create(Guid.NewGuid(), null!);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }

        [Test]
        public void Create_EmptyAccessToken_ReturnsFail()
        {
            var result = ProviderCredential.Create(Guid.NewGuid(), Array.Empty<byte>());

            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void Create_ExpiresAtWithLocalKind_ReturnsFail()
        {
            var local = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Local);
            var result = ProviderCredential.Create(Guid.NewGuid(), SampleToken(), expiresAtUtc: local);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("UTC"));
        }

        [Test]
        public void Create_ExpiresAtWithUtcKind_ReturnsSuccess()
        {
            var utc = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc);
            var result = ProviderCredential.Create(Guid.NewGuid(), SampleToken(), expiresAtUtc: utc);

            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Restore_CopiesAllFields()
        {
            var providerId = Guid.NewGuid();
            var updated = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            var expires = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc);

            var c = ProviderCredential.Restore(
                providerId, SampleToken(), new byte[] { 5 }, expires, "scope", "Bearer", updated);

            Assert.That(c.ProviderId, Is.EqualTo(providerId));
            Assert.That(c.AccessTokenEncrypted, Is.EqualTo(SampleToken()));
            Assert.That(c.RefreshTokenEncrypted, Is.EqualTo(new byte[] { 5 }));
            Assert.That(c.ExpiresAtUtc, Is.EqualTo(expires));
            Assert.That(c.Scope, Is.EqualTo("scope"));
            Assert.That(c.TokenType, Is.EqualTo("Bearer"));
            Assert.That(c.UpdatedUtc, Is.EqualTo(updated));
        }

        [Test]
        public void Restore_NullAccessToken_BecomesEmptyArray()
        {
            var c = ProviderCredential.Restore(
                Guid.NewGuid(), null!, null, null, null, null, DateTime.UtcNow);

            Assert.That(c.AccessTokenEncrypted, Is.Not.Null);
            Assert.That(c.AccessTokenEncrypted, Is.Empty);
        }
    }
}
