using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty; 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Passwort wird als SecureString gespeichert (verschlüsselt im Speicher)
        public SecureString Password { get; set; } = new SecureString();

        public User UserWithoutPassword 
        {
            get => new User
            {
                Id = this.Id,
                Username = this.Username,
                FirstName = this.FirstName,
                LastName = this.LastName,
                // Password wird nicht mitgegeben
                Password = null
            };
        }
    }
}
