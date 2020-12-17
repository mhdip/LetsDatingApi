using Lets_Dating.Data;
using Lets_Dating.DTOs;
using Lets_Dating.Entities;
using Lets_Dating.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lets_Dating.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext Context, ITokenService tokenService)
        {
            this._context = Context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Regsiter(RegisterDto registerDto)
        {
            // call prive User Exist function to check existing user
            if (await UserExist(registerDto.UserName)) return BadRequest("Username is Already Taken");

            // Insert User with username, password and password salt
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                passwordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Because we generate JWT, so while user regsiter a token will generated for specific time
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        //Check User Exist or not return true or false
        private async Task<bool> UserExist(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Check user while login
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid Username");

            // if user log in successfull then reverse the security, check password salt and then converte the it into password Hast
            using var hmac = new HMACSHA512(user.passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i =0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.passwordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
