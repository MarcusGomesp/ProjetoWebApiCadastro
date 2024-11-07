using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WFConfin.Models;

namespace WFConfin.Services
{
    public class TokenService
    {
        private readonly IConfiguration _cofiguration;

        public TokenService(IConfiguration cofiguration)
        {
            _cofiguration = cofiguration;
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var chave = Encoding.ASCII.GetBytes(_cofiguration.GetSection("Chave").Get<string>()); // conteudo em BYTES  ------------pegando informacao da chave no appsetings   -- 

            var TokenDescriptor = new SecurityTokenDescriptor() //dados que vai conter no token
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim (ClaimTypes.Name, usuario.Login.ToString()), //dados do usuario Nome
                        new Claim (ClaimTypes.Role, usuario.Funcao.ToString()), // funcao

                    }
                ),
                Expires = DateTime.UtcNow.AddHours(2), // Informar quando o token expira
                SigningCredentials = new SigningCredentials(       //credencial
                    new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature //informando a assinatura de credencial usando a variavel "CHAVE"
                )

            };

            var token = tokenHandler.CreateToken(TokenDescriptor); //criando token
            return tokenHandler.WriteToken(token);
        }
    }
}
