using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFConfin.Models
{
    public class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [StringLength(20, ErrorMessage = "O campo Telefone deve ter até 20 caracteres")]
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        [StringLength(20, ErrorMessage = "O campo Gênero deve ter até 20 caracteres")]
        public string Genero { get; set; }

        public Guid? CidadeId { get; set; } // ? pode recerber um valor nulo, não é um valor obrigatório

        public Pessoa()
        {
            Id = Guid.NewGuid();
        }

        public Cidade Cidade { get; set; } //Relacionamento Entity Framework
    }
}
