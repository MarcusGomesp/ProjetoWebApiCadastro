using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WFConfin.Data;
using WFConfin.Models;

namespace WFConfin.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CidadeController : Controller
    {
        private readonly WfConfinDbContext _context;

        public CidadeController(WfConfinDbContext context)
        {
            _context = context;
        }

        //criacao CRUD 


        [HttpGet]
        public async Task<IActionResult> GetCidades()
        {
            try
            {
                var reult = _context.Cidade.ToList();
                return Ok(reult);
            }
            catch (Exception e)
            {

                return BadRequest($"Erro na listagenem de Cidade. Exceção: {e.Message}");
            }
        }



        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostCidades([FromBody] Cidade valor)
        {

            try
            {
                await _context.Cidade.AddAsync(valor);
                var post = _context.SaveChanges();

                if (post == 1)
                {
                    return Ok("Sucesso, Cidade incluida.");
                }
                else
                {
                    return BadRequest("Erro, Cidade não incluida.");
                }

            }
            catch (Exception e)
            {

                return BadRequest($"Erro na inclusao de Cidade. Exceção: {e.Message}");
            }

        }




        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutCidades([FromBody] Cidade cidade)
        {

            try
            {
                _context.Cidade.Update(cidade);
                var valor = await _context.SaveChangesAsync();

                if (valor == 1)
                {
                    return Ok("Sucesso, Cidade alterada.");
                }
                else
                {
                    return BadRequest("Erro, Cidade não alterada.");
                }


            }
            catch (Exception e)
            {

                return BadRequest($"Erro na alteração de Cidade. Exceção: {e.Message}");
            }
        }


        [HttpDelete("{id}")] //Adicionar ID na URL
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteCidades([FromRoute] Guid id)
        {

            try
            {
                Cidade cidade = await _context.Cidade.FindAsync(id);
                if (cidade != null)
                {
                    _context.Cidade.Remove(cidade);
                    var result = await _context.SaveChangesAsync();

                    if (result == 1)
                    {
                        return Ok("Sucesso, Cidade excluida");
                    }
                    else
                    {
                        return BadRequest("Erro, Cidade não excluida");
                    }
                }
                else
                {
                    return NotFound("Erro, cidade não existe");
                }
            }
            catch (Exception e)
            {

                return BadRequest($"Erro na alteração de Cidade. Exceção: {e.Message}");
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCidade([FromRoute] Guid id)
        {

            try
            {
                Cidade cidade = await _context.Cidade.FindAsync(id);
                if (cidade != null)
                {
                    return Ok(cidade);
                }
                else
                {
                    return NotFound("Erro, cidade não existe");
                }
            }
            catch (Exception e)
            {

                return BadRequest($"Erro na consulta de Cidade. Exceção: {e.Message}");
            }

        }


        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetCidadePesquisa([FromQuery] string valor) //info q qremos pesquisar
        {
            try
            {

                //Query Criteria
                var lista = from o in _context.Cidade.ToList() //consulta banco
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                            || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de cidade. Exceção: {e.Message}");

            }
        }




        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetCidadePaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                var lista = from o in _context.Cidade.ToList() 
                            select o;


                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Nome.ToUpper().Contains(valor.ToUpper())
                             || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
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

                var paginacaoResponse = new PaginacaoResponse<Cidade>(lista, qtde, skip, take);// classe generica

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de cidade. Exceção: {e.Message}");

            }
        }

    }
}
