using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using UserAPI.Model;
using UserAPI.Model.Db;
using UserAPI.Repo;
using UserAPI.rsa;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("controller")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        private readonly IUserRepository _userRepository;

        public LoginController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            
            _userRepository = userRepository;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] LoginModel userLogin)
        {
            try
            {
                
                var roleId = _userRepository.UserCeck(userLogin.Email, userLogin.Password);
                var user = new UserModel { Email = userLogin.Email, Role = roleId};
                var token = GenerateToken(user);
                return Ok(token);
            }catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        private string GenerateToken(UserModel user)
        {
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var key = new RsaSecurityKey(RsaTools.GetPrivateKey());
            var cradentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: cradentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
