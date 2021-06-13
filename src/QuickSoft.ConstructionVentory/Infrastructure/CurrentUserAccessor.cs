using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shyjus.BrowserDetection;

namespace QuickSoft.ConstructionVentory.Infrastructure
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBrowserDetector _browserDetector;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, IBrowserDetector browserDetector)
        {
            _httpContextAccessor = httpContextAccessor;
            _browserDetector = browserDetector;
        }

        public string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
        
        public string GetCurrentUserType()
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Typ)
                ?.Value;
        }

        public IBrowser GetUserAgent()
        {
            return _browserDetector.Browser;
        }

        public string GetUserIp()
        {
            var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4();
            return remoteIpAddress?.ToString();
        }

        public int GetAuditId()
        {
            
            return int.Parse(_httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)
                ?.Value ?? string.Empty);
        }
    }
}