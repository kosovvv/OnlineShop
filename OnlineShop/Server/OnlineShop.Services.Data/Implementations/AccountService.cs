using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Services.Data.Extensions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;
using System.Security.Claims;

namespace OnlineShop.Services.Data.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal User)
        {
            var user = await userManager.FindByEmailFromClaimsPrincipal(User);
            var roles = await userManager.GetRolesAsync(user);

            return new UserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        public async Task<ReturnAddressDto> GetUserAddress(ClaimsPrincipal User)
        {
            var user = await userManager.FindUserByClaimsPrincipleWithAddress(User);

            return mapper.Map<Address, ReturnAddressDto>(user.Address);
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            var roles = await userManager.GetRolesAsync(user);

            if (user == null)
            {
                return null;
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return null;
            }

            return new UserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return null;
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

        public async Task<ReturnAddressDto> UpdateUserAdresss(ClaimsPrincipal User, ReturnAddressDto address)
        {
            var user = await userManager.FindUserByClaimsPrincipleWithAddress(User);

            user.Address = mapper.Map<ReturnAddressDto, Address>(address);

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return mapper.Map<Address, ReturnAddressDto>(user.Address);
            }

            return null;
        }
    }
}
