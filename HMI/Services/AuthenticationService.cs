using HMI.Models;
using Microsoft.AspNetCore.Identity;

namespace HMI.Services
{
    public class AuthenticationService :IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationStatus> RegisterAsync(RegisterDto model)
        {
            var status = new AuthenticationStatus();
            var userExists = await _userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User already exist";

                return status;
            }

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User creation failed";

                return status;
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            }

            if (await _roleManager.RoleExistsAsync(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.StatusMessage = "You have registered successfully";

            return status;
        }
    }
}
