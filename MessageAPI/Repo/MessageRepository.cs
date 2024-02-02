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

        public List<MessageModel> GetUnreadMessage(UserModel user)
        {
            throw new NotImplementedException();
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
