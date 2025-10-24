using FAI.Core.Entities;
using FAI.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace FAI.Common.Services
{
    public class UserService : IUserService
    {
        //Mockups für Benutzer
        private readonly List<User> users =
        [
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Username = "Test",
                Password = new System.Net.NetworkCredential("Test", "12345").SecurePassword
            }
        ];
            
        public async Task<User> Authenticate(string username, string password, CancellationToken cancellationToken = default)
        {
                // Suche nach Benutzer mit dem angegebenen Benutzernamen
                var user = users.SingleOrDefault(x => string.Compare(x.Username, username, true) == 0 &&
                                                      new NetworkCredential(x.Username, x.Password).Password == password);

                if (user == null)
                {
                    return user;
                }

                return await Task.FromResult(user.UserWithoutPassword);
        }
    }
}
