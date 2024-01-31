using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI.Model;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestrictedController : ControllerBase
    {
        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminEndPoint()
        {
            var currentuser = GetCurrenUser();
            return Ok($"Hi {currentuser.Role}");
        }

        [HttpGet]
        [Route("Users")]
        [Authorize(Roles = "Administrator, User")]
        public IActionResult UserEndPoint()
        {
            var currentuser = GetCurrenUser();
            return Ok($"Hi {currentuser.Role}");
        }

        private UserModel GetCurrenUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    Name = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = (UserRole)Enum.Parse(typeof(UserRole), userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value)
                };
            }
            return null;
        }
    }
}
