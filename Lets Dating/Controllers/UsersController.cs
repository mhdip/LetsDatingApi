using Lets_Dating.Data;
using Lets_Dating.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets_Dating.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;                         
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {  
            var users = await _context.Users.ToListAsync();
            return users;
        }

        
        // api/users/3 -> find the id of 3 users
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUsersById(int id)
        {
            return await _context.Users.FindAsync(id);

        }
    }
}
