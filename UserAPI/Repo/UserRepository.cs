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

        private ChatDbContext _chatDbContext;
        public UserRepository(IMapper mapper, ChatDbContext chatDbContext)
        {
            this._mapper = mapper;
            this._chatDbContext = chatDbContext;
        }
        public List<UserModel> GetUsers()
        {
            using (_chatDbContext)
            {
                var temp = _chatDbContext.Users.ToList();

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
            
            using (_chatDbContext)
            {
                if(roleId == UserRole.Administrator)
                {
                    var c = _chatDbContext.Users.Count(x => x.RoleId == UserRole.Administrator);
                    if (c > 0)
                    {
                        throw new Exception("Администратор должен быть один");
                    }
                }

                if (_chatDbContext.Users.FirstOrDefault(x => x.Name == name) !=null)
                {
                    throw new Exception("Пользователь с таким именем существует");
                }

                if (_chatDbContext.Users.FirstOrDefault(x => x.Email == email) != null)
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

                _chatDbContext.Add(user);
                _chatDbContext.SaveChanges();
            }
        }


        public UserRole UserCeck(string email, string password)
        {
            using(_chatDbContext)
            {
                var user = _chatDbContext.Users.FirstOrDefault(x => x.Email == email);
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


        public void UserDelete(string email)
        {
            using (_chatDbContext)
            {
                var user = _chatDbContext.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    throw new Exception("Пользователь не существует");
                }
                else
                {
                    _chatDbContext.Users.Remove(user);
                    _chatDbContext.SaveChanges() ;
                }
            }
                
        }
    }
}
