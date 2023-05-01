using IT120P.Models;

namespace IT120P.Services
{
    public class UserService
    {
        private readonly DeviceDbContext _dbContext;

        public UserService(DeviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User Authenticate(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
            {
                return null;
            }

            // remove password before returning
            user.Password = null;

            return user;
        }

    }
}
