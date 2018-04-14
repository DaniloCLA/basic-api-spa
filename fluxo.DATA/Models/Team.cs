using System.Collections.Generic;

namespace fluxo.DATA.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCustom { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public ICollection<TeamAssignment> UsersAssigned { get; set; }
    }
}