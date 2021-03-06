using System.Threading.Tasks;
using DatingApplication.api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApplication.api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user1= await _context.Users.FirstOrDefaultAsync(x=>x.Username == username);

           if(user1==null)
           return null;

           if(!VerifyPasswordHash(password,user1.PasswordHash, user1.PasswordSalt))
           return null;

           return user1;
        }
         private bool VerifyPasswordHash(string password, byte[] passwordHash,  byte[] pappasswordSalt)
         {
              using(var hmac=new System.Security.Cryptography.HMACSHA512(pappasswordSalt))
               {
                   var computedHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                   for(int i=0;i<computedHash.Length;i ++)
                   {
                        if(computedHash[i]!=passwordHash[i])
                        return false;
                   }
                    passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               }
               return true;
          }
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password,out passwordHash, out passwordSalt);
            user.PasswordHash=passwordHash;
            user.PasswordSalt=passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.Username==username))
                return true;

            return false;
        }
        public void CreatePasswordHash(string password,out byte[] passwordHash, out byte[] pappasswordSalt)
          {
               using(var hmac=new System.Security.Cryptography.HMACSHA512())
               {
                    pappasswordSalt=hmac.Key;
                    passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               }
          }
    }
}