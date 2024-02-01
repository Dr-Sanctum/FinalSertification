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
    public class GetUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public GetUserController(IUserRepository userRepository)
        {

            
            _userRepository = userRepository;
        }


        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public ActionResult<List<UserModel>> GetUsers()
        {
            try
            {
                var result = _userRepository.GetUsers();
                
                return result;
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }
    }
}
