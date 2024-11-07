using System.ComponentModel.DataAnnotations;

namespace WFConfin.Models
{
    public class Usuario
    {
        [Key]
        [Required(ErrorMessage = "O campo Id é obrigatório")]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Login deve ter entre 3 e 20 caracteres")]
        public string Login { get; set; }


        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Senha deve ter entre 3 e 20 caracteres")]
        public string Password { get; set; }


        [Required(ErrorMessage = "O campo Função é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Função deve ter entre 3 e 20 caracteres")]
        public string Funcao { get; set; }

    }
}
