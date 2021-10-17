using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskManager.Api
{
    public class TokenService
    {
        public static string CriarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var chaveCriptoEmBytes = Encoding.ASCII.GetBytes(TokenJWT.ChaveJWT);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chaveCriptoEmBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
