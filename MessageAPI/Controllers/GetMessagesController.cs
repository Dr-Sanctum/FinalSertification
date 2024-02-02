using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MessageAPI.Model;
using MessageAPI.Repo;

namespace MessageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetMessagesController : ControllerBase
    {
        
        private readonly IMessageRepository _messageRepository;

        public GetMessagesController( IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        

        [HttpPost]
        [Route("GetMessages")]
        [Authorize]
        public ActionResult<List<MessageModel>> GetMessages()
        {
            try
            {
                var currentuser = _messageRepository.GetCurrenUser(HttpContext);

                var result = _messageRepository.GetUnreadMessage(currentuser);

                return result;
            }
            catch (Exception e)
            {

                return StatusCode(500, e);
            }

        }

    }
}
