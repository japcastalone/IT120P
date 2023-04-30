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
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }
    }
}
