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
        private readonly UserManager<UserController> _UserManager;
        private readonly SignInManager<UserController> _SignInManager;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, UserManager<UserController> userManager,
        SignInManager<UserController> signInManager)
        {
            _logger = logger;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        public async Task<string> Register([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            var user = new AppUser()
            {
                FullName = model.FullName,
                Email = model.FullName,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };
            var result = _UserManager.CreateAsync(user, model.Password);
        }

    }
}
