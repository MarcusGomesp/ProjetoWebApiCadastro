using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConfin.Data;
using WFConfin.Models;

namespace WFConfin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContaController : Controller
    {
        private readonly WfConfinDbContext _context;

        public ContaController(WfConfinDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetContas()
        {
            try
            {
                var result = _context.Conta.ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na busca de Contas. Exceção: {e.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostConta([FromBody] Conta conta)
        {
            try
            {
                await _context.Conta.AddAsync(conta);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return Ok("Sucesso, Conta incluida.");
                }
                else
                {
                    return BadRequest("Erro, Conta não incluida.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na postagem de Conta. Exceção: {e.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutConta([FromBody] Conta conta)
        {
            try
            {
                _context.Conta.Update(conta);
                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    return Ok("Sucesso, Conta alterada.");
                }
                else
                {
                    return BadRequest("Erro, Conta não alterada.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alterção de Conta. Exceção: {e.Message}");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteConta([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _context.Conta.FindAsync(id);

                if (conta != null)
                {
                    _context.Conta.Remove(conta);
                    var valor = await _context.SaveChangesAsync();

                    if (valor == 1)
                    {
                        return Ok("Sucesso, conta Excluida.");
                    }
                    else
                    {
                        return BadRequest("Erro, conta não existe.");
                    }
                }
                else
                {
                    return NotFound("Erro, conta não existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro ao Excluir uma Conta. Exceção: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConta([FromRoute] Guid id)
        {

            try
            {
                Conta conta = await _context.Conta.FindAsync(id);

                if (conta != null)
                {
                    return Ok(conta);
                }
                else
                {
                    return NotFound("Erro, conta não encontrada");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de conta. Exceção: {e.Message}");
            }
        }


        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetContaPesquisa([FromQuery] string valor) //info q qremos pesquisar
        {
            try
            {

                //Query Criteria
                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            where o.Descricao.ToUpper().Contains(valor.ToUpper())
                            || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de Conta. Exceção: {e.Message}");

            }
        }



        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetContaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Descricao.ToUpper().Contains(valor.ToUpper())
                            || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;
                }


                if (ordemDesc)
                {
                    lista = from o in lista
                            orderby o.Descricao descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Descricao ascending
                            select o;
                }

                var qtde = lista.Count();

                lista = lista
                    .Skip((skip - 1) * take)
                    .Take(take)
                    .ToList();

                var paginacaoResponse = new PaginacaoResponse<Conta>(lista, qtde, skip, take);// classe generica

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de Conta. Exceção: {e.Message}");

            }
        }



        [HttpGet("Pessoa/{pessoaId}")]
        public async Task<IActionResult> GetContasPesoa([FromRoute] Guid pessoaId) //info q qremos pesquisar
        {
            try
            {

                //Query Criteria
                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            where o.PessoaId == pessoaId
                            select o;

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de Conta por Pessoa. Exceção: {e.Message}");

            }
        }
    }
}
