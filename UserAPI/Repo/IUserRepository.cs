using UserAPI.Model.Db;

namespace UserAPI.Repo
{
    public interface IUserRepository
    {
        public void UserAdd(string name, string email, string password, RoleId roleId);
        public RoleId UserCeck(string email, string password);
    }
}
