namespace fluxo.DATA.Models
{
    public class TeamAssignment
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public bool IsLead { get; set; }
    }
}