using API_Core.BindingModel;
using API_Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [HttpPost("RegisterUser")]
        public async Task<string> Register([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                var User = new AppUser()
                {
                    FullName = model.FullName,
                    Email = model.FullName,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };
                var result = await _UserManager.CreateAsync(User, model.Password);
                if (result.Succeeded)
                {
                    return await Task.FromResult("User has been created successfully");
                }
                return await Task.FromResult(string.Join(",", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }

        }
    }
}
