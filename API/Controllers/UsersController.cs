using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        // api/Users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        [Authorize]
        [HttpGet("{id}")]
        // api/Users/{id}
        public async Task< ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        [HttpGet("GetFullSentence")]
        // api/Users/sentence+
        public async Task<ActionResult<string>> GetFullSentence()
        {
            return await Task.Run(() =>
            {
                string listUser = string.Empty;
                List<AppUser> list = _context.Users.ToList();
                foreach (AppUser user in list)          
                    listUser = listUser + user.Id + " - " + user.UserName + Environment.NewLine;             
                return listUser;
            });
            
        }
    }
}