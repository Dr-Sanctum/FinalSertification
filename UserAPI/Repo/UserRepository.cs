using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Model;
using UserAPI.Model.Db;

namespace UserAPI.Repo
{
    public class UserRepository : IUserRepository
    {
        private IMapper _mapper;
        
        
        public UserRepository(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public List<UserModel> GetUsers()
        {
            using (var context = new ChatDbContext())
            {
                var temp = context.Users.ToList();

                var result = new List<UserModel>();
                foreach (var item in temp)
                {
                    result.Add(_mapper.Map<UserModel>(item));
                }
                return result;
            }
        }

        public void UserAdd(string name, string email, string password, UserRole roleId)
        {
            
            using (var context  = new ChatDbContext())
            {
                if(roleId == UserRole.Administrator)
                {
                    var c = context.Users.Count(x => x.RoleId == UserRole.Administrator);
                    if (c > 0)
                    {
                        throw new Exception("Администратор должен быть один");
                    }
                }

                if (context.Users.FirstOrDefault(x => x.Name == name) !=null)
                {
                    throw new Exception("Пользователь с таким именем существует");
                }

                if (context.Users.FirstOrDefault(x => x.Email == email) != null)
                {
                    throw new Exception("Пользователь с таким почтой существует");
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


        public UserRole UserCeck(string email, string password)
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
