using DevagramCSharp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevagramCSharp.Services
{
    public class TokenService
    {
        public static string CriarToken (Usuario usario)
        {
            var tokenHandler = new JwtSecurityTokenHandler ();
            var chaveCriptografia = Encoding.ASCII.GetBytes(ChaveJWT.ChaveSecreta);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Sid, usario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usario.Nome)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chaveCriptografia), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
