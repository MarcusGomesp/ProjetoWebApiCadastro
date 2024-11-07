using System.ComponentModel.DataAnnotations;

namespace WFConfin.Models
{
    public class Estado
    {
        [Key] //Indicando Chave Primaria
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo Sigla deve ter 2 caracteres")] //Indicar quantos caracteres sera Infomado
        public string Sigla { get; set; }



        [Required(ErrorMessage ="O campo Nome é obrigatório")] //Campo Obrigatorio
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo Nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

    }
}
