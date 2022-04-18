using API_Core.BindingModel;
using API_Core.Data.Entities;
using API_Core.DTO;
using API_Core.Enums;
using API_Core.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
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
        private readonly JWTConfig _JWTConfig;
        public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,IOptions<JWTConfig> jwtconfig)
        {
            _logger = logger;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _JWTConfig = jwtconfig.Value;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("RegisterUser")]
        public async Task<object> Register([FromBody] AddUpdateRegisterUserBindingModel model)
         {
            try
            {
                var User = new AppUser()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    UserName=model.Email,
                    NormalizedEmail=model.Email
                };
                var result = await _UserManager.CreateAsync(User, model.Password);
                if (result.Succeeded)
                {
                    ResponseModel rm = new ResponseModel(ResponseCode.Ok, "User has been created successfully", null);
                    return await Task.FromResult(rm);
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error,"",result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception e)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error,e.Message, null));
            }

        }
        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            var users = _UserManager.Users.Select(x => new DTOUsers(x.FullName, x.Email, x.UserName,  x.DateModified));
            return await Task.FromResult(new ResponseModel(ResponseCode.Ok,"",users));
        
        }
        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginBindingModel ob)
        {
            try
            {
                if (ob.Email == "" || ob.Password == "")
                {
                    return await Task.FromResult("Email or Password field is empty");
                }
                var result = await _SignInManager.PasswordSignInAsync(ob.Email, ob.Password, false, false);
                if (result.Succeeded)
                {
                    var appuser = await _UserManager.FindByEmailAsync(ob.Email);
                    var user = new DTOUsers(appuser.FullName, appuser.Email, appuser.Email, appuser.DateCreated);
                    user.token = GenerateToken(appuser);
                    return await Task.FromResult(new ResponseModel(ResponseCode.Ok, "", user));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid UserName/Password", null));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_JWTConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
                Issuer=_JWTConfig.Issuer,
                Audience=_JWTConfig.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
