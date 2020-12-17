using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets_Dating.DTOs
{
    public class UserDto
    {
        // This DTO Generated for Token, below object will return when user logged in or register
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
