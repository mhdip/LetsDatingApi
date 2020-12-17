using Lets_Dating.Entities;
using Lets_Dating.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lets_Dating.Services
{
    public class TokenService : ITokenService
    {
        // initialize private key from package system.identityModeltoken for JWT- Json Web Token
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
             // since our key's type is string and symmetric key is byte, that's why we encode this 
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            // what claim we want to put in this token
            var claims = new List<Claim>
            {
                // Registerd claim name from identity model token
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // We Need some Credential for this token, we need key which symmetric key and security algorithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // we Need Token Descriptor where we get actual, subject, token validity, and credential
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            // We Need Create Token Hanlder whichn handle the token / create the token through tokenDescriptor
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the Token and return it's value
            return tokenHandler.WriteToken(token);
        }
    }
}
