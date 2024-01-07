using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Entities; // Ensure this using statement is included
using API.Interfaces; // Ensure this using statement is included for ITokenservice

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenservice _tokenService;
     public AccountController(DataContext context , ITokenservice tokenService) // constructor
     {
            _tokenService = tokenService;
            _context = context;

     }
     [HttpPost("register")]//POST:api/account/register

     public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
     {
        if(await UserExists(registerDto.Username)) return BadRequest("Username is already taken");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName =registerDto.Username.ToLower(),
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
     }
     [HttpPost("login")]
     public async Task<ActionResult<UserDto>>Login(LoginDto logindto)
     {
       var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == logindto.Username);

        if(user == null) return Unauthorized("Invalid User Name");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

        for (int i = 0 ; i<computedHash.Length; i++)
        {
            if(computedHash[i] !=user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
     }
        private async Task<bool>UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}