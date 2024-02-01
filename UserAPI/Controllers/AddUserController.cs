using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Model;
using UserAPI.Repo;

namespace UserAPI.Controllers
{
    public class AddUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AddUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddUser")]
        public ActionResult AddUser([FromBody] LoginModel userLogin, string name)
        {
            try
            {
                _userRepository.UserAdd(name, userLogin.Email, userLogin.Password, UserRole.User);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }
    }
}
