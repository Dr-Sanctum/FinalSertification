using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using MessageAPI.Model;
using MessageAPI.Model.Db;

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

        public List<MessageModel> GetUnreadMessage()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(MessageModel sendMessage)
        {
            using (_messageDbContext)
            {
                var message = new Message()
                {
                    Unread = true,
                    EmailFrom = "a@a",
                    EmailTo = "u@u",
                    Text = "Привет"
                };

                _messageDbContext.Add(message);
                _messageDbContext.SaveChanges();
            }
        }
    }
}
