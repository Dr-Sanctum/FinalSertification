using System.Security.Cryptography;
using System.Text;
using UserAPI.Model.Db;

namespace UserAPI.Repo
{
    public class UserRepository : IUserRepository
    {
        public void UserAdd(string name, string email, string password, RoleId roleId)
        {
            
            using (var context  = new ChatDbContext())
            {
                if(roleId == RoleId.Admin)
                {
                    var c = context.Users.Count(x => x.RoleId == RoleId.Admin);
                    if (c > 0)
                    {
                        throw new Exception("Администратор должен быть один");
                    }
                }
                var user = new User();
                user.Name = name;
                user.Email = email;
                user.RoleId = roleId;

                user.Salt = new byte[16];
                new Random().NextBytes(user.Salt);
                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();

                user.Password = shaM.ComputeHash(data);

                context.Add(user);
                context.SaveChanges();
            }
        }

        public RoleId UserCeck(string email, string password)
        {
            using(var context = new ChatDbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                var brassword = shaM.ComputeHash(data);
                if (user.Password.SequenceEqual(brassword))
                {
                    return user.RoleId;
                }
                else
                {
                    throw new Exception("Wrong password");
                }
            }
        }
    }
}
