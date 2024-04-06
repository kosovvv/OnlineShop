using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;

namespace OnlineShop.WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await userManager.FindByEmailFromClaimsPrincipal(User);
            var roles = await userManager.GetRolesAsync(user);


            return new UserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user,roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        } 

        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<ReturnAddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserByClaimsPrincipleWithAddress(User);
             
            return mapper.Map<Address, ReturnAddressDto>(user.Address);
        }

        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<ReturnAddressDto>> UpdateUserAdresss(ReturnAddressDto address)
        {
            var user = await userManager.FindUserByClaimsPrincipleWithAddress(User);

            user.Address = mapper.Map<ReturnAddressDto, Address>(address);

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(mapper.Map<Address, ReturnAddressDto>(user.Address));
            }

            return BadRequest("Problem updating user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            var roles = await userManager.GetRolesAsync(user);

            if (user == null) 
            {
                return Unauthorized(new ApiResponse(401));
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }

            return new UserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = ["Email in use"]
                });
            }

            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) 
            {
                return BadRequest(new ApiResponse(400));
            }

            await userManager.AddToRoleAsync(user, Roles.User);
            var roles = await userManager.GetRolesAsync(user);

            return new UserDto
            {
                Email = user.DisplayName,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };  
        }
    }
}
