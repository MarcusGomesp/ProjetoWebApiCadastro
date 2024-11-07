using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using WFConfin.Data;
using WFConfin.Models;
using WFConfin.Services;

namespace WFConfin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Precisa 
    public class UsuarioController : Controller
    {
        private readonly WfConfinDbContext _context;
        private readonly TokenService _service;

        public UsuarioController(WfConfinDbContext context, TokenService service)
        {
            _context = context;
            _service = service;
        }



        [HttpPost]
        [Route("Login")]
        [AllowAnonymous] //quem nao tem authentificação
        public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
        {
            var usuario = _context.Usuario.Where(x => x.Login == usuarioLogin.Login).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound("Usuário Inválido");
            }

            var passwordHash = MD5Hash.CalcHash(usuarioLogin.Password); //Info da senha

            if (usuario.Password != passwordHash)
            {
                return BadRequest("Senha Inválida");
            }

            var token = _service.GerarToken(usuario);

            usuario.Password = "";

            var result = new UsuarioResponse()
            {
                Usuario = usuario,
                Token = token
            };

            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetUsurio()
        {
            try
            {
                var result = _context.Usuario.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na busca de Usuario. Exceção: {e.Message}");
            }
        }


        [HttpPost]
        //[Authorize(Roles = "Gerente,Empregado")] //quem pode acessar
        public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var listUsuario = _context.Usuario.Where(x => x.Login == usuario.Login).ToList();


                if (listUsuario.Count > 0)
                {
                    return BadRequest("Erro, Informação de login inválido.");
                }

                string passworHash = MD5Hash.CalcHash(usuario.Password);

                usuario.Password = passworHash; //armazenando o Hash de criptografia 

                await _context.Usuario.AddAsync(usuario);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return Ok("Sucesso, Usuário incluido.");
                }
                else
                {
                    return BadRequest("Erro, Usuário não incluido.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na postagem de Usuário. Exceção: {e.Message}");
            }
        }


        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")] //quem pode acessar
        public async Task<IActionResult> PutUsuario([FromBody] Usuario usuario)
        {
            try
            {
                string passworHash = MD5Hash.CalcHash(usuario.Password);

                usuario.Password = passworHash; //armazenando o Hash de criptografia 

                _context.Usuario.Update(usuario);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return Ok("Sucesso, Usuário alterado.");
                }
                else
                {
                    return BadRequest("Erro, Usuário não alterado.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alterção de Usuário. Exceção: {e.Message}");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")] //quem pode acessar
        public async Task<IActionResult> DeleteUsuario([FromRoute] Guid id)
        {
            try
            {
                Usuario usuario = await _context.Usuario.FindAsync(id);

                if (usuario != null)
                {
                    _context.Usuario.Remove(usuario);
                    var valor = await _context.SaveChangesAsync();

                    if (valor == 1)
                    {
                        return Ok("Sucesso, Usuário Excluido.");
                    }
                    else
                    {
                        return BadRequest("Erro, Usuário não existe.");
                    }
                }
                else
                {
                    return NotFound("Erro, Usuário não existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao Excluir um Usuário. Exceção: {e.Message}");
            }
        }






    }

}
