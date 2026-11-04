namespace CloudSyncPulse.DataAccess.Sqlite.Infrastructures
{
    public class ResDb
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        private ResDb(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error ?? string.Empty;
        }

        public static ResDb Ok() => new(true, string.Empty);
        public static ResDb Fail(string error) => new(false, error ?? "Неизвестная ошибка.");
    }

    public class ResDb<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string Error { get; }

        private ResDb(bool isSuccess, T? value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error ?? string.Empty;
        }

        public static ResDb<T> Ok(T value) => new(true, value, string.Empty);
        public static ResDb<T> Fail(string error) => new(false, default, error ?? "Неизвестная ошибка.");
    }
}
