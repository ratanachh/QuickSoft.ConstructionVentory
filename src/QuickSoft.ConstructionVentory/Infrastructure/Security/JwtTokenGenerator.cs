using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace QuickSoft.ConstructionVentory.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string CreateToken(string username, string userType, string auditId)
        {
            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Typ, userType),
                new Claim(JwtRegisteredClaimNames.Aud, auditId),
                new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claim,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public IJwtTokenGenerator ValidTokenTime(int second)
        {
            _jwtOptions.ValidFor = TimeSpan.FromSeconds(second);
            return this;
        }
    }
}