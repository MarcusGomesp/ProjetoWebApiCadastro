using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using WFConfin.Data;
using WFConfin.Models;

namespace WFConfin.Controllers
{
    [ApiController] // IP PORTA
    [Route("api/[controller]")] //API/nomeDaClasse
    [Authorize]
    public class EstadoController : Controller
    {

        private readonly WfConfinDbContext _context; //atributo _context

        public EstadoController(WfConfinDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                var result = _context.Estado.ToList(); // receber uma listagem dos estados

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagenem de Estados. Exceção: {e.Message}");

            }
        }



        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostEstado([FromBody] Estado estado) //vir do body //vir infomacao da classe
        {
            try
            {
                await _context.Estado.AddAsync(estado); //incluir estado
                var valor = await _context.SaveChangesAsync(); //salvar Informacoes 

                if (valor == 1)
                {
                    return Ok("Sucesso, Estado incluido.");
                }
                else
                {
                    return BadRequest("Erro, Estado não incluido.");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, Estado não incluido. Exceção: {e.Message}");

            }
        }


        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutEstado([FromBody] Estado estado) //vir do body //vir infomacao da classe
        {
            try
            {
                _context.Estado.Update(estado); //incluir estado 
                var valor = await _context.SaveChangesAsync(); //salvar Informacoes 

                if (valor == 1)
                {
                    return Ok("Sucesso, Estado alterado.");
                }
                else
                {
                    return BadRequest("Erro, Estado não alterado.");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, Estado não alterado. Exceção: {e.Message}");

            }
        }




        [HttpDelete("{sigla}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteEstado([FromRoute] string sigla) //vir da rota //vir infomacao da classe
        {
            try
            {
                var estado = await _context.Estado.FindAsync(sigla);


                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla)) //validar informacoes
                {
                    _context.Estado.Remove(estado);
                    var valor = await _context.SaveChangesAsync();

                    if (valor == 1)
                    {
                        return Ok("Sucesso, Estado excluido");
                    }
                    else
                    {
                        return BadRequest("Erro,estado não excluido");
                    }

                }
                else
                {
                    return NotFound("Erro, Estado não esxite");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, Estado não alterado. Exceção: {e.Message}");

            }
        }


        [HttpGet("{sigla}")]
        public async Task<IActionResult> GetEstado([FromRoute] string sigla) //vir da rota //vir infomacao da classe
        {
            try
            {
                var estado = await _context.Estado.FindAsync(sigla); //verificar no banco com a chave 


                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla)) //validar informacoes
                {
                    return Ok(estado);

                }
                else
                {
                    return NotFound("Erro, Estado não existe");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, consulta de estado. Exceção: {e.Message}");

            }
        }



        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetEstadoPesquisa([FromQuery] string valor)
        {
            try
            {

                //Query Criteria
                var lista = from o in _context.Estado.ToList() //consulta banco
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                //Entity
                /*lista =  _context.Estado
                      .Where(o => o.Sigla.ToUpper().Contains(valor.ToUpper())
                          || o.Nome.ToUpper().Contains(valor.ToUpper())
                             )
                      .ToList();*/

                //Expression
                /*Expression <Func<Estado, bool>> expressao =o => true;
            expressao = o => o.Sigla.ToUpper().Contains(valor.ToUpper())
                    || o.Nome.ToUpper().Contains(valor.ToUpper());

            lista = _context.Estado.Where(expressao); */

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de estado. Exceção: {e.Message}");

            }
        }




        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstadoPaginacao([FromQuery] string? valor, int skip, int take, bool ordemDesc)
        {
            try
            {

                //Query Criteria
                var lista = from o in _context.Estado.ToList() //consulta banco
                            select o;

                if (!String.IsNullOrEmpty(valor))
                {
                    lista = from o in lista
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                             || o.Nome.ToUpper().Contains(valor.ToUpper())
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

                var paginacaoResponse = new PaginacaoResponse<Estado>(lista, qtde, skip, take);// classe generica

                return Ok(paginacaoResponse);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de estado. Exceção: {e.Message}");

            }
        }

    }

}