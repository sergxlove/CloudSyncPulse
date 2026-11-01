namespace CloudSyncPulse.Core.Infrastructures
{
    public class ResModel<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string Error { get; set; } = string.Empty;

        public ResModel(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        public ResModel(string error)
        {
            IsSuccess = false;
            Error = error;
        }
    }
}
