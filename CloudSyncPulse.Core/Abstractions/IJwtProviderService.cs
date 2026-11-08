using CloudSyncPulse.Core.Requests;

namespace CloudSyncPulse.Core.Abstractions
{
    public interface IJwtProviderService
    {
        string? GenerateToken(JwtRequest request);
    }
}