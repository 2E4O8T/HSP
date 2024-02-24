using HMI.Models;
using HMI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid) 
            { 
                return View(model); 
            }
            
            var result = await _authenticationService.RegisterAsync(model);
            TempData["msg"] = result.StatusMessage;
            return RedirectToAction(nameof(Register));
        }
    }
}
