using UserAPI.Model;
using UserAPI.Model.Db;

namespace UserAPI.Repo
{
    public interface IUserRepository
    {
        public void UserAdd(string name, string email, string password, UserRole roleId);

        public void UserDelete(string email);

        public List<UserModel> GetUsers();
        public int GetId(string email);
        public UserRole UserCeck(string email, string password);
    }
}
