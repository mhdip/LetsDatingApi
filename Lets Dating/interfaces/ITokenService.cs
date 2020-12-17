using Lets_Dating.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets_Dating.interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);    
    }
}
