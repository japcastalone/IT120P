using IT120P.Data;
using IT120P.Models;
using System;
using System.Linq;

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

        public bool Register(User user)
        {
            // check if username already exists
            if (_dbContext.Users.Any(x => x.Username == user.Username))
            {
                return false;
            }

            // set default authority to 0
            user.Autho = default;

            // add user to database
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return true;
        }

        public User GetById(int id)
        {
            return _dbContext.Users.Find(id);
        }
    }
}
