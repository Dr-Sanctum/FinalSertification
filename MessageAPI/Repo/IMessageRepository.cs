using MessageAPI.Model;
using MessageAPI.Model.Db;

namespace MessageAPI.Repo
{
    public interface IMessageRepository
    {
        public void SendMessage(MessageModel sendMessage);
        public List<MessageModel> GetUnreadMessage(UserModel user);
       
    }
}
