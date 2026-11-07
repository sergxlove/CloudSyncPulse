using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Models;

namespace CloudSyncPulse.Tests.UnitTests
{
    [TestFixture]
    public class SyncRunTests
    {
        [Test]
        public void Create_ValidData_ReturnsSuccess()
        {
            var ruleId = Guid.NewGuid();
            var before = DateTime.UtcNow.AddSeconds(-1);
            var result = SyncRun.Create(ruleId, SyncRunTrigger.Schedule);
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.That(result.IsSuccess, Is.True);
            var v = result.Value!;
            Assert.That(v.SyncRuleId, Is.EqualTo(ruleId));
            Assert.That(v.Status, Is.EqualTo(SyncRunStatus.Running));
            Assert.That(v.Trigger, Is.EqualTo(SyncRunTrigger.Schedule));
            Assert.That(v.FinishedUtc, Is.Null);
            Assert.That(v.FilesTotal, Is.EqualTo(0));
            Assert.That(v.FilesSucceeded, Is.EqualTo(0));
            Assert.That(v.FilesFailed, Is.EqualTo(0));
            Assert.That(v.BytesUploaded, Is.EqualTo(0));
            Assert.That(v.ErrorMessage, Is.Null);
            Assert.That(v.StartedUtc, Is.InRange(before, after));
        }

        [Test]
        public void Create_EmptyRuleId_ReturnsFail()
        {
            var result = SyncRun.Create(Guid.Empty, SyncRunTrigger.Manual);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Empty);
        }

        [Test]
        public void Create_InvalidTrigger_ReturnsFail()
        {
            var result = SyncRun.Create(Guid.NewGuid(), (SyncRunTrigger)999);

            Assert.That(result.IsSuccess, Is.False);
        }

        [TestCase(SyncRunTrigger.Schedule)]
        [TestCase(SyncRunTrigger.Manual)]
        [TestCase(SyncRunTrigger.Retry)]
        public void Create_AllValidTriggers_ReturnSuccess(SyncRunTrigger trigger)
        {
            var result = SyncRun.Create(Guid.NewGuid(), trigger);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value!.Trigger, Is.EqualTo(trigger));
        }

        [Test]
        public void Restore_CopiesAllFields()
        {
            var id = Guid.NewGuid();
            var ruleId = Guid.NewGuid();
            var started = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var finished = new DateTime(2024, 1, 1, 0, 5, 0, DateTimeKind.Utc);

            var r = SyncRun.Restore(
                id, ruleId, started, finished, SyncRunStatus.Partial, SyncRunTrigger.Retry,
                10, 7, 3, 12345, "some error");

            Assert.That(r.Id, Is.EqualTo(id));
            Assert.That(r.SyncRuleId, Is.EqualTo(ruleId));
            Assert.That(r.StartedUtc, Is.EqualTo(started));
            Assert.That(r.FinishedUtc, Is.EqualTo(finished));
            Assert.That(r.Status, Is.EqualTo(SyncRunStatus.Partial));
            Assert.That(r.Trigger, Is.EqualTo(SyncRunTrigger.Retry));
            Assert.That(r.FilesTotal, Is.EqualTo(10));
            Assert.That(r.FilesSucceeded, Is.EqualTo(7));
            Assert.That(r.FilesFailed, Is.EqualTo(3));
            Assert.That(r.BytesUploaded, Is.EqualTo(12345));
            Assert.That(r.ErrorMessage, Is.EqualTo("some error"));
        }

        [Test]
        public void Restore_DoesNotMutateDates()
        {
            var started = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var r = SyncRun.Restore(Guid.NewGuid(), Guid.NewGuid(), started, null,
                SyncRunStatus.Running, SyncRunTrigger.Schedule, 0, 0, 0, 0, null);

            Assert.That(r.StartedUtc, Is.EqualTo(started));
            Assert.That(r.FinishedUtc, Is.Null);
        }
    }
}
