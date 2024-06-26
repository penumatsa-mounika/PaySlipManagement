﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaySlipManagement.BAL.Implementations;
using PaySlipManagement.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaySlipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string SecretKey = "your_secret_key_here_1234567890_1234567890_1234567890_"; // 256-bit key
        private readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));


        private readonly UserBALRepo _userBALRepo;
        public UserController()
        {
            _userBALRepo = new UserBALRepo();
        }

        [HttpGet("GetAllUsers")]
        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _userBALRepo.GetAllAsync();
        }

        [HttpGet("GetUserById/{Id}")]
        public async Task<Users> GetByIdUser(int Id)
        {
            Users u = new Users();
            u.Id = Id;
            return await _userBALRepo.GetByIdAsync(u);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] Users _user)
        {
            if (_user != null)
            {
                var data = await _userBALRepo.Create(_user);
                if (data)
                {
                    return Ok("Register Successfull");
                }
                else
                {
                    return Ok("Check the credentials are correct");
                }

            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(Users _user)
        {
            if (_user != null)
            {
                var data = await _userBALRepo.Update(_user);
                if (data)
                {
                    return Ok("Update Successfull");
                }
                else
                {
                    return Ok("Check the credentials are correct");
                }
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }

        }
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(Users _user)
        {
            if (_user != null)
            {
                var data = await _userBALRepo.Delete(_user);
                if (data)
                {
                    return Ok("Delete Successfull");
                }
                else
                {
                    return Ok("Make sure the credentials are correct");
                }
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(Users _user)
        {
            if (_user != null)
            {
                var data = await _userBALRepo.UserValidateUserCredentials(_user);
                if (data != null)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name,data.Email),
                    //new Claim(ClaimTypes.Role, data.Role)
                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { User = _user.Email,/*User1 =_user.Password*/ Token = tokenString });
                }
                else
                {
                    return Ok("Make sure the credentials are correct");
                }
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }

    }
}

