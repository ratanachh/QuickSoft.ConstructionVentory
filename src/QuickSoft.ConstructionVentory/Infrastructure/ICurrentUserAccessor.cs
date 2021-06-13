using Shyjus.BrowserDetection;

namespace QuickSoft.ConstructionVentory.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUsername();
        string GetCurrentUserType();

        IBrowser GetUserAgent();
        string GetUserIp();
        int GetAuditId();
    }
}