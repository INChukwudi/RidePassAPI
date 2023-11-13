using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RidePassAPI.Contracts.ServiceContracts;
using RidePassAPI.Dtos.Auth;
using RidePassAPI.Models.IdentityModels;
using RidePassAPI.Responses;
using System.Security.Claims;

namespace RidePassAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IHashids _hashids;

        public UserAccountsController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ITokenService tokenService, IHashids hashids)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _hashids = hashids;
        }

        [HttpGet("getCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDetailsDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) { return Unauthorized(); }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return Unauthorized(); }

            return Ok(new UserDetailsDto
            {
                Email = user.Email,
                UserId = _hashids.Encode((int)user.UserId),
                UserType = user.UserType,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email!.ToLower().Trim());
            if (user == null)
            {
                return Unauthorized(new ErrorResponse(401, "Login failed!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ErrorResponse(401, "Login Failed!"));
            }

            return Ok(new UserDetailsDto
            {
                Email = user.Email,
                UserId = _hashids.Encode((int)user.UserId),
                UserType = user.UserType,
                Token = _tokenService.CreateToken(user)
            });
        }
    }
}
