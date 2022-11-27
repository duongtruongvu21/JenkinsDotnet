using System.Security.Cryptography;
using System.Text;
using JD.API.Data;
using JD.API.Data.DTO;
using JD.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JD.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("UserRegister")]
        public IActionResult Register([FromBody] UserDTO userRegister)
        {
            userRegister.Username = userRegister.Username.ToLower();
            if (_context.Users.Any(u => u.Username == userRegister.Username))
            {
                return BadRequest(false);
            }

            using var hmac = new HMACSHA512();
            var passwordByte = Encoding.UTF8.GetBytes(userRegister.Password);
            var newUser = new User()
            {
                Username = userRegister.Username,
                PasswordSalt = hmac.Key,
                PasswordHashed = hmac.ComputeHash(passwordByte)
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(true);
        }


        [HttpPost("UserLogin")]
        public IActionResult UserLogin([FromBody] UserDTO userLogin)
        {
            userLogin.Username = userLogin.Username.ToLower();
            var currentUser = _context.Users.
                FirstOrDefault(u => u.Username == userLogin.Username);

            if (currentUser == null)
            {
                return Unauthorized(-1);
            }

            using (var hmac = new HMACSHA512(currentUser.PasswordSalt))
            {
                var passwordBytes = hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(userLogin.Password));

                for (int i = 0; i < currentUser.PasswordHashed.Length; i++)
                {
                    if (currentUser.PasswordHashed[i] != passwordBytes[i])
                    {
                        return Unauthorized(0);
                    }
                }

                return Ok(1);
            }
        }

        [HttpGet("getAllUser")]
        public IActionResult GetAllUser()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpGet("Jenkins")]
        public IActionResult Jenkins()
        {
            return Ok("Jenkins Test 1");
        }
    }
}