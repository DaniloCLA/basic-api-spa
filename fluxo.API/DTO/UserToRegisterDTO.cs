using System.ComponentModel.DataAnnotations;

namespace fluxo.API.DTO
{
    public class UserToRegisterDTO
    {
        [Required(ErrorMessage="Usuário inválido")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage="Email inválido")]
        [EmailAddress(ErrorMessage="Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage="Senha inválida")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "A senha deve conter entre 5 e 25 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage="Organização inválida")]
        public string OrganizationName { get; set; }
    }
}