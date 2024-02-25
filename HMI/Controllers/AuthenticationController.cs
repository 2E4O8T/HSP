using HMI.Models;
using HMI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterDto model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            var result = await _authService.RegisterAsync(model);
            TempData["msg"] = result.StatusMessage;
            
            return RedirectToAction(nameof(Register));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                
            var result = await _authService.LoginAsync(model);

            var jwtToken = result.Token;

            if (result.StatusCode == 1 && jwtToken != string.Empty)
            {
                return RedirectToAction("Index", "Home", new { token = jwtToken});
            }
            else
            {
                TempData["msg"] = result.StatusMessage;

                return RedirectToAction(nameof(Login));
            }
        }

        //[Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
