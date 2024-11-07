using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WFConfin.Data;
using WFConfin.Models;

namespace WFConfin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PessoaController : Controller
    {
        private readonly WfConfinDbContext _context;

        public PessoaController(WfConfinDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetPessoa()
        {
            try
            {
                var result = _context.Pessoa.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na busca de Pessoas. Exceção: {e.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                await _context.Pessoa.AddAsync(pessoa);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return Ok("Sucesso, Pessoa incluida.");
                }
                else
                {
                    return BadRequest("Erro, Pessoa não incluida.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na postagem de Pessoa. Exceção: {e.Message}");
            }
        }




        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                _context.Pessoa.Update(pessoa);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return Ok("Sucesso, Pessoa alterada.");
                }
                else
                {
                    return BadRequest("Erro, Pessoa não alterada.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alterção de Pessoa. Exceção: {e.Message}");
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeletePessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);

                if (pessoa != null)
                {
                    _context.Pessoa.Remove(pessoa);
                    var valor = await _context.SaveChangesAsync();

                    if (valor == 1)
                    {
                        return Ok("Sucesso, Pessoa Excluida.");
                    }
                    else
                    {
                        return BadRequest("Erro, Pessoa não existe.");
                    }
                }
                else
                {
                    return NotFound("Erro, Pessoa não existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao Excluir uma Pessoa. Exceção: {e.Message}");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetPessoa([FromRoute] Guid id)
        {

            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);

                if (pessoa != null)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return NotFound("Erro, pessoa não encontrada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de Pessoa. Exceção: {e.Message}");
            }
        }


        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetPessoaPesquisa([FromQuery] string valor) //info q qremos pesquisar
        {
            try
            {

                //Query Criteria
                var lista = from o in _context.Pessoa.ToList() //consulta banco
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.Telefone.ToUpper().Contains(valor.ToUpper())
                            || o.Email.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de pessoa. Exceção: {e.Message}");

            }
        }




        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                var lista = from o in _context.Pessoa.ToList() //consulta banco
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.Telefone.ToUpper().Contains(valor.ToUpper())
                             || o.Email.ToUpper().Contains(valor.ToUpper())
                            select o;
                }

                if (ordemDesc)
                {
                    lista = from o in lista
                            orderby o.Nome descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Nome ascending
                            select o;
                }

                var qtde = lista.Count();

                lista = lista
                    .Skip((skip - 1) * take)
                    .Take(take)
                    .ToList();

                var paginacaoResponse = new PaginacaoResponse<Pessoa>(lista, qtde, skip, take);// classe generica

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de Pessoa. Exceção: {e.Message}");

            }
        }
    }
}
