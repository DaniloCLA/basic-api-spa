using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using AutoMapper;

using fluxo.DATA.Repository;
using fluxo.API.DTO;
using fluxo.DATA.Models;
using fluxo.API.Helpers;

namespace fluxo.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }

        [HttpGet("exists/{email}")]
        public async Task<IActionResult> Exists(string email) 
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            bool exists = await _repo.UserExists(email);

            if (!exists) return BadRequest();

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserToRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(userDTO.Email))
                userDTO.Email = userDTO.Email.ToLower();

            if (await _repo.UserExists(userDTO.Email))
                ModelState.AddModelError("Email", "Email já existente no sistema");

            var userToCreate = _mapper.Map<User>(userDTO);

            var createdUser = await _repo.Register(userToCreate, userDTO.Password, userDTO.OrganizationName);

            if (createdUser.Id == 0)
                return BadRequest();

            var userToReturn = _mapper.Map<UserToListDTO>(createdUser);

            return Ok();

            //return CreatedAtRoute("GetUser", new {controller = "Users", id = createdUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserToLoginDTO loginDTO)
        {
            var userFromRepo = await _repo.Login(loginDTO.Email.ToLower(), loginDTO.Password);

            if (userFromRepo == null || !userFromRepo.IsValid())
                return Unauthorized();

            var token = this.GenerateToken(userFromRepo);
            var user = _mapper.Map<UserToListDTO>(userFromRepo);

            return Ok(new { token, user });
        }

        private string GenerateToken(User user) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();            
            //TODO: Use Environment
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:ApiKey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.DisplayName)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}