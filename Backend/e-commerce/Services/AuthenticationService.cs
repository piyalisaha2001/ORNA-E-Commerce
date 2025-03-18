using e_commerce.Data;
using e_commerce.models;
using System.Linq;

namespace e_commerce.Services
{
    public class AuthenticationService
    {
        private readonly AppDbContext _dbContext;

        public AuthenticationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.username == username);

            if (user != null)
            {
                if (password != null)
                {
                    // Compare the hash of the entered password with the hash of the password from the database
                    bool passwordIsValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                    return passwordIsValid;
                }
            }

            return false;
        }
    }
}
