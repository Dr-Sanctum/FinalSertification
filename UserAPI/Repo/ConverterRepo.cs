using UserAPI.Model.Db;
using UserAPI.Model;

namespace UserAPI.Repo
{
    public static class ConverterRepo
    {
        public static UserRole RoleToRoleId(RoleId id)
        {
            if (id == RoleId.Admin)
            {
                return UserRole.Administrator;
            }
            else
            {
                return UserRole.User;
            }
        }
    }
}
