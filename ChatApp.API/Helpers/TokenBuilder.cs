using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.API.Helpers
{
    public static class TokenBuilder
    {
        private const string keyString = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed";
        public static readonly byte[] symmetricKeyBytes = Encoding.ASCII.GetBytes(keyString);
        public static readonly SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(symmetricKeyBytes);
        public static readonly SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        internal static TokenValidationParameters tokenValidationParams;
        
        public static void ConfigureJwtAuthentication(this IServiceCollection services)
        {
            tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://www.tulparyazilim.com.tr",
                ValidateLifetime = true,
                ValidAudience = "https://www.tulparyazilim.com.tr",
                ValidateAudience = true,
                RequireSignedTokens = true, 
                IssuerSigningKey = signingCredentials.Key,
                ClockSkew = TimeSpan.FromMinutes(0)
            };
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParams;
                #if PROD || UAT
                options.IncludeErrorDetails = false;
                #elif DEBUG
                options.RequireHttpsMetadata = false;
                #endif
            });
        }
        public static string CreateJsonWebToken(
               string username,
               IEnumerable<string> roles,
               string audienceUri,
               string issuerUri,
               Guid applicationId,
               DateTime expires,
               string deviceId = null,
               bool isReAuthToken = false)
        {
            var claims = new List<Claim>();
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            } 
            var jwt = new JwtSecurityToken(issuerUri, audienceUri, claims, DateTime.UtcNow, expires, signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
