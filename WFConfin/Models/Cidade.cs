using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WFConfin.Models
{
    public class Cidade
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatorio")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo estado deve ter 2 caracteres")]
        public string EstadoSigla { get; set; }    //relacionamento entre calsses

        public Cidade()
        {
            Id = Guid.NewGuid();
        }

        [JsonIgnore] // ignorar dados Nulos
        public Estado Estado { get; set; } //Relacionamento Entity Framework


    }
}
