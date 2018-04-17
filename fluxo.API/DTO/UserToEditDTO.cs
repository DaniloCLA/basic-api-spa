using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fluxo.API.DTO
{
    public class UserToEditDTO
    {
        [Required(ErrorMessage="Email inválido")]
        [EmailAddress(ErrorMessage="Email inválido")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage="Nome Completo inválido")]
        public string FullName { get; set; }

        [Required(ErrorMessage="Usuário inválido")]
        public string DisplayName { get; set; }
        
        public int[] TeamIds { get; set; }
    }
}