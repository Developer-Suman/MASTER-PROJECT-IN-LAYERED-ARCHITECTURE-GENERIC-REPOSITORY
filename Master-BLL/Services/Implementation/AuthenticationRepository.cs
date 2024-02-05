using Master_BLL.Services.Interface;
using Master_DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_BLL.Services.Implementation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            
        }

        public async Task<IdentityResult> AssignRoles(ApplicationUser user, string rolename)
        {
            return await _userManager.AddToRoleAsync(user, rolename);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser username, string password)
        {
            return await _userManager.CheckPasswordAsync(username, password);
        }

        public async Task<bool> CheckRoleAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<IdentityResult> CreateRoles(string role)
        {
            return await _roleManager.CreateAsync(new IdentityRole(role));
            
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return default!;
            }

            return user;
        }

        public async Task<ApplicationUser?> FindByIdAsync(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }

        public async Task<ApplicationUser> FindByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user is null )
            {
                return default!;
            }
            return user;
        }

        public async Task<List<ApplicationUser>?> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser username)
        {
            return await _userManager.GetRolesAsync(username);
        }

        public Task UpdateUserAsync(ApplicationUser user)
        {
            return _userManager.UpdateAsync(user);
        }
    }
}
