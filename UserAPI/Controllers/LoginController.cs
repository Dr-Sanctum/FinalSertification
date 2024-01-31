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
        [Route("AddAdmin")]
        public ActionResult AddAdmin([FromBody] LoginModel userLogin, string name)
        {
            try
            {
                _userRepository.UserAdd(name, userLogin.Email, userLogin.Password, RoleId.Admin);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AddUser")]
        public ActionResult AddUser([FromBody] LoginModel userLogin, string name)
        {
            try
            {
                _userRepository.UserAdd(name, userLogin.Email, userLogin.Password, RoleId.User);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] LoginModel userLogin)
        {
            try
            {
                var roleId = _userRepository.UserCeck(userLogin.Email, userLogin.Password);
                var user = new UserModel { Email = userLogin.Email, Role = RoleToRoleId(roleId) };
                var token = GenerateToken(user);
                return Ok(token);
            }catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private static UserRole RoleToRoleId(RoleId id)
        {
            if (id == RoleId.Admin)
            {
                return UserRole.Administrator;
            }
            else
            {
                return UserRole.User;
            }


        }

        private string GenerateToken(UserModel user)
        {
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var key = new RsaSecurityKey(RsaTools.GetPrivateKey());
            var cradentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
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
