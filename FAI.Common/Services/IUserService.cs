using FAI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Common.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password, CancellationToken cancellationToken = default);
    }
}
