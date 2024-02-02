using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MessageAPI.Model;
using MessageAPI.Repo;

namespace MessageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendMessageController : ControllerBase
    {
        
        private readonly IMessageRepository _messageRepository;

        public SendMessageController( IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        

        [HttpPost]
        [Route("SendMessage")]
        [Authorize]
        public IActionResult SendMessage(string sendTo, string text)
        {
          
                var currentuser = GetCurrenUser();

                var result = new MessageModel()
                {
                    EmailFrom = currentuser.Email,
                    EmailTo = sendTo,
                    Text = text
                };

                _messageRepository.SendMessage(result);
            


            return Ok();
        }


        private UserModel GetCurrenUser()
        {
            var id = HttpContext.User.Identity as ClaimsIdentity;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var x = new UserModel
                {
                    Id = int.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value),
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                };

                return x;
            }
            return null;
        }
    }
}
