using Master_BLL.DTOs.RegistrationDTOs;
using Master_BLL.Services.Interface;
using Master_DAL.Abstraction;
using Master_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MASTER_PROJECT_IN_LAYERED_ARCHITECTURE_GENERIC_REPOSITORY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtProvider _jwtProvider;

        public AccountController(IAuthenticationRepository authenticationRepository, IJwtProvider jwtProvider)
        {
            _authenticationRepository = authenticationRepository;
            _jwtProvider = jwtProvider;
            
        }

        #region Authentication
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegistrationCreateDTOs registrationCreateDTOs)
        {
            var userExists = await _authenticationRepository.FindByNameAsync(registrationCreateDTOs.Username);
            if(userExists is not null)
            {
                return BadRequest("User Already Exists");
            }

            var emailExists = await _authenticationRepository.FindByEmailAsync(registrationCreateDTOs.Email);
            if(emailExists is not null)
            {
                return BadRequest("Email Already Exists");
            }

            ApplicationUser user = new()
            {
                UserName = registrationCreateDTOs.Username,
                Email = registrationCreateDTOs.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _authenticationRepository.CreateUserAsync(user, registrationCreateDTOs.Password);

            if(!result.Succeeded)
            {
                return BadRequest("An error occured while adding user");
            }

            //await _authenticationRepository.CreateRoles(user, registrationCreateDTOs.Role);

           
            return Ok(result);

        }
        #endregion


        #region Create Roles
        [HttpGet("CreateRole")]
        public async Task<ActionResult> CreateRolesAsync(string rolename)
        {
            var roleExists = await _authenticationRepository.CheckRoleAsync(rolename);
            if(!roleExists)
            {
                var result = await _authenticationRepository.CreateRoles(rolename);
                if(result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return Conflict("Role Already Exists");
            }

        }
        #endregion

        #region Assign Roles
        [HttpPost("AssignRoles")]
        public async Task<ActionResult> AssignRolesAsync(string userId, string rolename)
        {
            var user = await _authenticationRepository.FindByIdAsync(userId);
            if(user is not null)
            {
                var result = await _authenticationRepository.AssignRoles(user,rolename);
                if(result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            else
            {
                return NotFound("User not Found");
            }
        }
        #endregion

        #region RefreshToken
        public async Task<IActionResult> NewRefreshToken(string refreshtoken,string token)
        {
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(token);
            if(principal is null)
            {
                return BadRequest("Invalid Token");

            }

            string username = principal.Identity!.Name!;
            if(username is null)
            {
                return BadRequest("Invalid Token");
            }

            var user = await _authenticationRepository.FindByNameAsync(username);

            if(user is null || user.RefreshToken != refreshtoken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid Access Token and refresh Token");
            }


            var roles = await _authenticationRepository.GetRolesAsync(user);
            var newToken = _jwtProvider.Generate(user, roles);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken();
            user.RefreshToken= newToken;
            await _authenticationRepository.UpdateUserAsync(user);

            return Ok(new {Token = newToken, RefreshToken = newRefreshToken });
        }
        #endregion
    }
}
