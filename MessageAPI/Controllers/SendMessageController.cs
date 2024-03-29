﻿using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var currentuser = _messageRepository.GetCurrenUser(HttpContext);

                var result = new MessageModel()
                {
                    EmailFrom = currentuser.Email,
                    EmailTo = sendTo,
                    Text = text
                };

                _messageRepository.SendMessage(result);
                return Ok();
            }
            catch (Exception e)
            {

                return StatusCode(500, e);
            }
                
        }

    }
}
