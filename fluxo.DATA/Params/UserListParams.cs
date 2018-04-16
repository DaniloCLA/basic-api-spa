namespace fluxo.DATA.Params
{
    public class UserListParams : BaseParams
    {
        public int UserId { get; set; }
        public string NameContains { get; set; }
        public string OrderBy { get; set; }
    }
}