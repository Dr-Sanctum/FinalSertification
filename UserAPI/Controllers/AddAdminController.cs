using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Model;
using UserAPI.Repo;

namespace UserAPI.Controllers
{
    public class AddAdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AddAdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddAdmin")]
        public ActionResult AddUser([FromBody] LoginModel userLogin, string name)
        {
            try
            {
                _userRepository.UserAdd(name, userLogin.Email, userLogin.Password, UserRole.Administrator);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }
    }
}
