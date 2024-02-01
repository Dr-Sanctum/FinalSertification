using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Model;
using UserAPI.Repo;

namespace UserAPI.Controllers
{
    public class DeleteUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        
        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "Administrator")]
        public ActionResult UserDelete(string email)
        {
            try
            {
                _userRepository.UserDelete(email);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }
    }
}
