﻿using Master_BLL.Services.Interface;
using Master_DAL.Models;
using Microsoft.AspNetCore.Identity;
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

        public AuthenticationRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
        }
        public async Task<bool> CheckPasswordAsync(ApplicationUser username, string password)
        {
            return await _userManager.CheckPasswordAsync(username, password);
        }

        public async Task<IdentityResult> CreateRoles(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
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

        public async Task<ApplicationUser> FindByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user is null )
            {
                return default!;
            }
            return user;
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
