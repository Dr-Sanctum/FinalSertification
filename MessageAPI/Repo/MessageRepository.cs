using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using MessageAPI.Model;
using MessageAPI.Model.Db;
using System.Security.Claims;

namespace MessageAPI.Repo
{
    public class MessageRepository : IMessageRepository
    {
        private IMapper _mapper;

        private MessageDbContext _messageDbContext;
        public MessageRepository(IMapper mapper, MessageDbContext chatDbContext)
        {
            this._mapper = mapper;
            this._messageDbContext = chatDbContext;
        }

        public UserModel GetCurrenUser(HttpContext httpcontext)
        {
            var id = httpcontext.User.Identity as ClaimsIdentity;
            var identity = httpcontext.User.Identity as ClaimsIdentity;
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

        public List<MessageModel> GetUnreadMessage(UserModel user)
        {
            using (_messageDbContext)
            {
                foreach (var item in _messageDbContext.Messages)
                {
                    item.Unread = false;
                }
                var temp = _messageDbContext.Messages.Where(x => x.Unread == true).ToList();

                var result = new List<MessageModel>();
                foreach (var item in temp)
                {
                    result.Add(_mapper.Map<MessageModel>(item));
                }

                foreach (var item in _messageDbContext.Messages)
                {
                    item.Unread = false;
                }
                _messageDbContext.SaveChanges();
                return result;
            }
        }

        public void SendMessage(MessageModel sendMessage)
        {
            using (_messageDbContext)
            {

                _messageDbContext.Add(_mapper.Map<Message>(sendMessage));
                _messageDbContext.SaveChanges();
            }
        }
    }
}
