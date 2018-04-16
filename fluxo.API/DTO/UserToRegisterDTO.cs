using System.ComponentModel.DataAnnotations;

namespace fluxo.API.DTO
{
    public class UserToRegisterDTO
    {
        [Required(ErrorMessage="Email inválido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage="Senha inválida")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "A senha deve conter entre 4 e 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage="Usuário inválido")]
        public string DisplayName { get; set; }
    }
}