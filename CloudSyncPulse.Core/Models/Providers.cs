using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Infrastructures;

namespace CloudSyncPulse.Core.Models
{
    public class Providers
    {
        public Guid Id { get; private set; }
        public ProviderType Type { get; private set; }
        public string DisplayName { get; private set; } = string.Empty;
        public bool IsEnabled { get; private set; }
        public DateTime CreatedUtc { get; private set; }
        public DateTime UpdatedUtc { get; private set; }

        private Providers() { }

        public static ResModel<Providers> Create(
            ProviderType type,
            string displayName,
            bool isEnabled = true)
        {
            if (!Enum.IsDefined(typeof(ProviderType), type))
                return new ResModel<Providers>("Некорректный тип провайдера.");

            if (string.IsNullOrWhiteSpace(displayName))
                return new ResModel<Providers>("DisplayName обязателен.");

            if (displayName.Length > 200)
                return new ResModel<Providers>("DisplayName не должен превышать 200 символов.");

            var now = DateTime.UtcNow;
            return new ResModel<Providers>(new Providers
            {
                Id = Guid.NewGuid(),
                Type = type,
                DisplayName = displayName.Trim(),
                IsEnabled = isEnabled,
                CreatedUtc = now,
                UpdatedUtc = now
            });
        }

        public static Providers Restore(
            Guid id,
            ProviderType type,
            string displayName,
            bool isEnabled,
            DateTime createdUtc,
            DateTime updatedUtc)
        {
            return new Providers
            {
                Id = id,
                Type = type,
                DisplayName = displayName ?? string.Empty,
                IsEnabled = isEnabled,
                CreatedUtc = createdUtc,
                UpdatedUtc = updatedUtc
            };
        }
    }
}
