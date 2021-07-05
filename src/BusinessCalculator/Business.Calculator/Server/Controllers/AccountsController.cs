using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Business.Calculator.Server.Models;
using Calculator.Models.DatabaseModels.Identity;
using Microsoft.AspNetCore.Identity;
using RegisterModel = Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.RegisterModel;

namespace Business.Calculator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new ApplicationUser() { UserName = model.Input.Email, Email = model.Input.Email };

            var result = await _userManager.CreateAsync(newUser, model.Input.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Ok(new RegisterResult() {Successful = false, Errors = errors});
            }

            return Ok(new RegisterResult() {Successful = true});
        }
    }
}
