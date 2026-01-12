using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(int userId, string email, string role);
    }
}
