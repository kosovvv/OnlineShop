using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Address;

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

        public async Task<ReturnUserDto> GetCurrentUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(user);

            return new ReturnUserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        public async Task<ReturnAddressDto> GetUserAddress(string userId)
        {
            var user = await userManager.Users
               .Include(u => u.Address)
               .FirstOrDefaultAsync(u => u.Id == userId);

            return mapper.Map<Address, ReturnAddressDto>(user.Address);
        }

        public async Task<ReturnUserDto> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            var roles = await userManager.GetRolesAsync(user);

            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new LoginFailedException("Error logging in.");
            }

            return new ReturnUserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        public async Task<ReturnUserDto> Register(RegisterDto registerDto)
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
                var errorMessages = result.Errors.Select(e => e.Description);
                throw new SignUpFailedException("Error signing up: " + string.Join(", ", errorMessages));
            }

            await userManager.AddToRoleAsync(user, "User");
            var roles = await userManager.GetRolesAsync(user);

            return new ReturnUserDto
            {
                Email = user.DisplayName,
                Token = tokenService.CreateToken(user, roles.First()),
                Role = roles.First(),
                DisplayName = user.DisplayName,
            };
        }

        public async Task<ReturnAddressDto> UpdateUserAdresss(string userId, ReturnAddressDto address)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.Address = mapper.Map<ReturnAddressDto, Address>(address);

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new SavingUserAddressException("Error saving email address");
            }

            return mapper.Map<Address, ReturnAddressDto>(user.Address);
        }

    }
}
