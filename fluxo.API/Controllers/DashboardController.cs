using AutoMapper;
using fluxo.DATA.Repository;
using Microsoft.AspNetCore.Mvc;

namespace fluxo.API.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {        
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        public DashboardController(IAuthRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
    }
}