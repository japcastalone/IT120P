using IT120P.Data;
using IT120P.Models;

namespace IT120P.Services
{
    public class AuthService
    {
        private readonly DeviceDbContext _dbContext;

        public AuthService(DeviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Authenticate(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public bool Register(User user)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return false;
            }

            user.Autho = default;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
