using CloudSyncPulse.Core.Models;

namespace CloudSyncPulse.Tests.UnitTests
{
    [TestFixture]
    public class UsersTests
    {

        [Test]
        public void Create_ValidData_ReturnsSuccess()
        {
            var result = Users.Create("admin", "hash123");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Error, Is.Empty);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value!.Username, Is.EqualTo("admin"));
            Assert.That(result.Value.PasswordHash, Is.EqualTo("hash123"));
            Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Create_TrimsUsername()
        {
            var result = Users.Create("  admin  ", "hash");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value!.Username, Is.EqualTo("admin"));
        }

        [Test]
        public void Create_GeneratesNewIdEachTime()
        {
            var a = Users.Create("admin", "hash").Value!;
            var b = Users.Create("admin", "hash").Value!;

            Assert.That(a.Id, Is.Not.EqualTo(b.Id));
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyUsername_ReturnsFail(string? username)
        {
            var result = Users.Create(username!, "hash");

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void Create_UsernameTooLong_ReturnsFail()
        {
            var username = new string('a', 101);
            var result = Users.Create(username, "hash");

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("100"));
        }

        [Test]
        public void Create_UsernameExactlyMaxLength_ReturnsSuccess()
        {
            var username = new string('a', 100);
            var result = Users.Create(username, "hash");

            Assert.That(result.IsSuccess, Is.True);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Create_EmptyPasswordHash_ReturnsFail(string? hash)
        {
            var result = Users.Create("admin", hash!);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }


        [Test]
        public void Restore_CopiesAllFields()
        {
            var id = Guid.NewGuid();
            var user = Users.Restore(id, "admin", "hash");

            Assert.That(user.Id, Is.EqualTo(id));
            Assert.That(user.Username, Is.EqualTo("admin"));
            Assert.That(user.PasswordHash, Is.EqualTo("hash"));
        }

        [Test]
        public void Restore_NullStrings_BecomeEmpty()
        {
            var user = Users.Restore(Guid.NewGuid(), null!, null!);

            Assert.That(user.Username, Is.Empty);
            Assert.That(user.PasswordHash, Is.Empty);
        }
    }
}
