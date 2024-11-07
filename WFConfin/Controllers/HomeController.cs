using Microsoft.AspNetCore.Mvc;
using WFConfin.Models;

namespace WFConfin.Controllers
{
    [ApiController]
    [Route("[controller]")] // <--- usar o mesmo nome da classe 
    public class HomeController : Controller
    {

        private static List<Estado> listaEstados = new List<Estado>();


        [HttpGet("Estado")]
        public IActionResult GetEstados()
        {
            return Ok(listaEstados);
        }

        [HttpPost("Estado")]
        public IActionResult PostEstados([FromBody] Estado estado)
        {
            listaEstados.Add(estado);
            return Ok("Estado Cadastrado");
        }

            [HttpGet]
        public IActionResult GetInformacao()
        {
            var result = "retorno em texto";
            return Ok(result);
        }

        [HttpGet("Informação 2")]
        public IActionResult GetInformacao2()
        {

            var result = "retorno em texto2";
            return Ok(result);
        }

        [HttpGet("Informação3/{valor}")] // o que eu informar na url
        public IActionResult GetInformacao3([FromRoute] string valor)
        {

            var result = "retorno em texto3 - Valor: " + valor;
            return Ok(result);
        }


        [HttpPost("Informação4")]
        public IActionResult GetInformacao4([FromQuery] string valor) //Retorna um valor query uma consulta exp: ?valor=Teste
        {

            var result = "retorno em texto3 - Valor: " + valor;
            return Ok(result);
        }


        [HttpGet("Informação5")]
        public IActionResult GetInformacao5([FromHeader] string valor) //retorna um valor no Header
        {

            var result = "retorno em texto3 - Valor: " + valor;
            return Ok(result);
        }

        [HttpPost("Informação6")]
        public IActionResult GetInformacao6([FromBody] Corpo corpo) //retorna um valor que esta no body(Json ou XML)
        {

            var result = "retorno em texto3 - Valor: " + corpo.valor;
            return Ok(result);
        }

    }

    public class Corpo
    {
        public string valor { get; set; }
    }
}
