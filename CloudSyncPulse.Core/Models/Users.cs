using CloudSyncPulse.Core.Infrastructures;

namespace CloudSyncPulse.Core.Models
{
    public class Users
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        private Users() { }

        public static ResModel<Users> Create(string username, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new ResModel<Users>("Username обязателен.");
            if (username.Length > 100)
                return new ResModel<Users>("Username не должен превышать 100 символов.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                return new ResModel<Users>("PasswordHash обязателен.");

            return new ResModel<Users>(new Users
            {
                Id = Guid.NewGuid(),
                Username = username.Trim(),
                PasswordHash = passwordHash
            });
        }
    }
}
