using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System;

using fluxo.DATA.Repository;
using fluxo.DATA.Params;
using fluxo.API.Helpers;
using fluxo.API.DTO;
using fluxo.DATA.Models;
using System.Linq;

namespace fluxo.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuthRepository _authRepo;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepo, IAuthRepository authRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _authRepo = authRepo;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepo.GetUser(id);

            var userToReturn = _mapper.Map<UserToEditDTO>(user);

            return Ok(userToReturn);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserListParams userParams)
        {
            userParams.UserId = this.LoggedUser;
            var users = await _userRepo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserToListDTO>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserToEditDTO userDTO) {
            if (!string.IsNullOrEmpty(userDTO.Email))
                userDTO.Email = userDTO.Email.ToLower();

            if (await _authRepo.UserExists(userDTO.Email))
                ModelState.AddModelError("Email", "Email já existente no sistema");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = this.LoggedUser;
            var loggedUserFromRepo = await _userRepo.GetUser(this.LoggedUser);

            if (!loggedUserFromRepo.IsAdmin())
                return Unauthorized();

            var userToCreate = _mapper.Map<User>(userDTO);
            await _authRepo.AddMember(userToCreate, userDTO.Password, loggedUserFromRepo.Organization);

            var userToReturn = _mapper.Map<UserToListDTO>(userToCreate);

            return CreatedAtRoute("GetUser", new {controller = "Users", id = userToCreate.Id}, userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserToEditDTO userToEditDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userFromRepo = await _userRepo.GetUser(id);

            if (userFromRepo == null)
                return NotFound($"Usuário com ID {id} não encontrado");

            var currentUserId = this.LoggedUser;
            var loggedUserFromRepo = await _userRepo.GetUser(this.LoggedUser);
            if (loggedUserFromRepo.Id != userFromRepo.Id && !loggedUserFromRepo.IsAdmin())
                return Unauthorized();
            
            _mapper.Map(userToEditDTO, userFromRepo);
            await this.UpdateUserTeams(userFromRepo, userToEditDTO.TeamIds);

            if (await _userRepo.SaveAll())
                return NoContent();

            throw new Exception($"Falha ao salvar alterações para o usuário com ID {id}");
        }

        private async Task UpdateUserTeams(User userFromRepo, int[] newTeamAssignments)
        {
            var haveNewAssignments = newTeamAssignments != null && newTeamAssignments.Count() > 0;

            if (userFromRepo.TeamsAssigned != null)
            {
                //Delete Assignments
                foreach (var ta in userFromRepo.TeamsAssigned)
                {
                    if (!newTeamAssignments.Contains(ta.TeamId))
                        userFromRepo.TeamsAssigned.Remove(ta);
                }
            } else if (haveNewAssignments) {
                userFromRepo.TeamsAssigned = new List<TeamAssignment>();
            }

            //Add Assignments
            if (haveNewAssignments) {
                var currentTeamIds = userFromRepo.TeamsAssigned.Select(ta => ta.TeamId);

                foreach (var tid in newTeamAssignments)
                {
                    if (!currentTeamIds.Contains(tid))
                    {
                        var team = await _userRepo.GetTeam(tid);
                        userFromRepo.TeamsAssigned.Add(new TeamAssignment(){ User = userFromRepo, Team = team });
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var currentUserId = this.LoggedUser;
            var loggedUserFromRepo = await _userRepo.GetUser(this.LoggedUser);
            if (loggedUserFromRepo.Id != id && !loggedUserFromRepo.IsAdmin())
                return Unauthorized();

            var userFromRepo = await _userRepo.GetUser(id);
            if (userFromRepo == null)
                return NotFound($"Usuário com ID {id} não encontrado");
            if (userFromRepo.IsOwner())
                return BadRequest("O usuário mestre não pode ser deletado");

            //TODO: Can't delete if has tasks pending
            
            userFromRepo.TeamsAssigned = null;
            userFromRepo.IsDeleted = true;

            if (await _userRepo.SaveAll())
                return Ok();

            throw new Exception($"Falha ao salvar alterações para o usuário com ID {id}");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> RestoreUser(int id)
        {
            var currentUserId = this.LoggedUser;
            var loggedUserFromRepo = await _userRepo.GetUser(this.LoggedUser);
            if (loggedUserFromRepo.Id != id && !loggedUserFromRepo.IsAdmin())
                return Unauthorized();

            var userFromRepo = await _userRepo.GetUser(id, false);
            if (userFromRepo == null)
                return NotFound($"Usuário com ID {id} não encontrado");

            userFromRepo.IsDeleted = false;

            if (await _userRepo.SaveAll())
                return Ok();

            throw new Exception($"Falha ao salvar alterações para o usuário com ID {id}");
        }
    }
}