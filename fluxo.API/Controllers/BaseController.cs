using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace fluxo.API.Controllers
{
    public class BaseController : Controller
    {
        public int LoggedUser { get { return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); } }
    }
}