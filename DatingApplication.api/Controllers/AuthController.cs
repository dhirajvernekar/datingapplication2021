//using System.IdentityModel.Tokens.Jwt; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using DatingApplication.api.Data;
using DatingApplication.api.Dtos;

using DatingApplication.api.Models;

namespace DatingApp.api.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
    public class AuthController:ControllerBase
    {
    
         private readonly IAuthRepository _repo;
         private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            _repo=repo;
            _config=config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDTO)//
        {

            //validate request
            userForRegisterDTO.Username=userForRegisterDTO.Username.ToLower();
            if(await _repo.UserExists(userForRegisterDTO.Username))
            return BadRequest("user already exist");

            var userToCreate=new User
            {
                Username=userForRegisterDTO.Username
            };
            var createduser=await _repo.Register(userToCreate,userForRegisterDTO.Password);
            // var createduser=await _repo.Register(null,null);
             return StatusCode(201);

        }
       
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo=await _repo.Login(userForLoginDTO.Username,userForLoginDTO.Password);
            if(userFromRepo==null)
            return Unauthorized("unauthorized");

            var claims=new []
            {
                  new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                  new Claim(ClaimTypes.Name,userFromRepo.Username.ToString())
            };
         var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
         var creds=new SigningCredentials(key,Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);

         var tokenDescriptor=new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
         {
               Subject=new ClaimsIdentity(claims),
               Expires= DateTime.Now.AddDays(100),
               SigningCredentials=creds
         };

         var tokenHandler=new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
         var token=tokenHandler.CreateToken(tokenDescriptor);

         return Ok(new {
             token = tokenHandler.WriteToken(token)
         });
        }
    }
}