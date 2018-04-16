using System.Threading.Tasks;
using GoldenTicket.Models;
using GoldenTicket.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoldenTicket.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Technician> _signInManager;

        private readonly ILogger _logger;

        public AccountController(SignInManager<Technician> signInManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest, [FromQuery] string returnUrl = null)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, loginRequest.RememberMe, false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"{User.Identity.Name} logged in.");
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(TicketsController.All), "Tickets");
                }
            }

            return View(loginRequest);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}