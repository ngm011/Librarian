using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Librarian.ApiPortal.Auth
{
    public class JwtAuthenticator : IAuthenticator
    {
        private readonly string _apiKey;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public JwtAuthenticator(string secretKey, string apiKey)
        {
            if (secretKey is null) throw new ArgumentNullException(nameof(secretKey));
            if (String.IsNullOrWhiteSpace(secretKey)) throw new ArgumentException($"{nameof(secretKey)} has to be specified", nameof(secretKey));
            if (apiKey is null) throw new ArgumentNullException(nameof(apiKey));
            if (String.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException($"{nameof(apiKey)} has to be specified", nameof(apiKey));

            _apiKey = apiKey;
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

        public string Authenticate(string apiKey)
        {
            if (apiKey != _apiKey) return null;

            return GetSerializedToken();
        }

        public SymmetricSecurityKey SymmetricSecurityKey => _symmetricSecurityKey;

        private string GetSerializedToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, _apiKey)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
