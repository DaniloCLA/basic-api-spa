using System.ComponentModel.DataAnnotations;

namespace fluxo.API.DTO
{
    public class UserToLoginDTO
    {
        [Required(ErrorMessage="Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage="Senha inválida")]
        public string Password { get; set; }
        
    }
}