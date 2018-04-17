using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using fluxo.DATA.Context;
using fluxo.DATA.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace fluxo.DATA.Repository
{
    public class AuthRepository : IAuthRepository
    {
        protected readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Users
                .Include(p => p.OrganizationOwned)
                .Include(p => p.TeamsAssigned).ThenInclude(ta => ta.Team)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
           {
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for (int i = 0; i < computedHash.Length; i++)
               {
                   if (computedHash[i] != passwordHash[i]) return false;
               }
           }
           return true;
        }

        public async Task<User> AddMember(User user, string password, Organization organization)
        {
            this.InitiateUser(user, password);
            user.Organization = organization;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Register(User user, string password, string organizationName)
        {
            this.InitiateUser(user, password);

            using (var transaction = _context.Database.BeginTransaction())
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                
                await CreateOrganization(user, organizationName);

                transaction.Commit();
            }

            return user;
        }

        private void InitiateUser(User user, string password) {
            user.Created = DateTime.Now;

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512())
           {
               passwordSalt = hmac.Key;
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           }
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
                return true;
            
            return false;
        }

        public async Task CreateOrganization(User user, string organizationName) 
        {
            var organization = new Organization(){ Name = organizationName, Owner = user };
            var adminTeam = new Team { Name = "Admin", IsCustom = false };
            adminTeam.UsersAssigned = new List<TeamAssignment> { new TeamAssignment(){ User = user, Team = adminTeam, IsLead = true } };
            organization.Teams = new List<Team> { adminTeam };

            user.Organization = organization;
            
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
        }
    }
}