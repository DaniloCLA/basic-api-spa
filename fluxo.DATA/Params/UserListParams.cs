namespace fluxo.DATA.Params
{
    public class UserListParams : PageableParams
    {
        public int UserId { get; set; }
        public string NameContains { get; set; }
        public string OrderBy { get; set; }
    }
}