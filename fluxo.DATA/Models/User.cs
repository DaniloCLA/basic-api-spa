using System;
using System.Collections.Generic;

namespace fluxo.DATA.Models
{
    public class User
    {        
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public Organization OrganizationOwned { get; set; }
        public ICollection<TeamAssignment> TeamsAssigned { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}