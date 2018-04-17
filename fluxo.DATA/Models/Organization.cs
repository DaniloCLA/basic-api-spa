using System.Collections.Generic;

namespace fluxo.DATA.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}