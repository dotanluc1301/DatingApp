using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService token)
        {
            this._context = context;
            this._tokenService = token;
        }
        [HttpPost("register")]
        public async Task<ActionResult<DTOUser>> Register(DTORegister dTORegister)
        {
            if(await IsUnique(dTORegister.UserName)) return BadRequest("UserName is taken");

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = dTORegister.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dTORegister.Password)),
                PasswordSalt = hmac.Key
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new DTOUser{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<DTOUser>> Login(DTOLogin dTOLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName.ToLower() == dTOLogin.UserName.ToLower());
            if(user == null) return Unauthorized("Invalid User name or Password");
            var computedHash = new HMACSHA512(user.PasswordSalt)          
                                    .ComputeHash(Encoding.UTF8.GetBytes(dTOLogin.Password));
            if(!StructuralComparisons.StructuralEqualityComparer.Equals(computedHash,user.PasswordHash))
                return Unauthorized("Invalid User name or Password");

            return new DTOUser{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool>IsUnique(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}