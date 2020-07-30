using System.Threading.Tasks;
using Calculator.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        /// <summary>
        /// ctor for the <see cref="AccountController"/>
        /// </summary>
        /// <param name="userManager">The UserManager provided by the IdentityServer4</param>
        /// <param name="roleManager">The RoleManager provided by the IdentityServer4</param>
        /// <param name="emailSender">The EmailSender to send confirmation links or password reset instructions</param>
        /// <param name="loggerFactory">The LoggerFactory</param>
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        //{

        //}
    }
}
