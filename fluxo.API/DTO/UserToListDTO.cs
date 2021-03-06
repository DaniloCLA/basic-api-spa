using System;

namespace fluxo.API.DTO
{
    public class UserToListDTO
    {        
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastActive { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsValid { get; set; }
    }
}