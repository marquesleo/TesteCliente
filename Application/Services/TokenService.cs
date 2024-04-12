using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public static class TokenService
    {

        public class MeuToken
        {
            public string token { get; set; }
            public DateTime expira { get; set; }
        }
        public static MeuToken GenerateToken(string usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   
                    new Claim(ClaimTypes.Name,usuario),
                   

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                
            };
           var token = tokenHandler.CreateToken(tokenDescriptor);
            var meuToken = new MeuToken();
            meuToken.token = tokenHandler.WriteToken(token);
            meuToken.expira = tokenDescriptor.Expires.Value;
            return meuToken;
        }

        public static string GenerateToken(ClaimsPrincipal claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,claims.Identity.Name ),
                    new Claim(ClaimTypes.NameIdentifier, claims.Identity.Name )
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");



            return principal;

        }


        public static List<(string, string)> _refreshTokens = new();


        public static void SaveRefreshToken(string username,string refreshToken)
        {
            _refreshTokens.Add(new(username, refreshToken));
        }

        public static string GetRefreshToken(string username)
        {
            return _refreshTokens.FirstOrDefault(x => x.Item1 == username).Item2;
        }
        public static void DeleteRefreshToken(string username,string refreshToken)
        {
            var item = _refreshTokens.FirstOrDefault(x => x.Item1 == username && x.Item2 == refreshToken);
            _refreshTokens.Remove(item);
        }




        
    }
}
