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
            if (_dbContext.Users.Any(u => u.Username == user.Username))
            {
                return false;
            }
            else
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return true;
            }
        }

        internal bool Authenticate(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
