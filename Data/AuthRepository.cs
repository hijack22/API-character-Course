using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_Course.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API_Course.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        private readonly IConfiguration _configuration; 
        public AuthRepository(DataContext context, IConfiguration configuration ) 
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<serviceResponse<string>> Login(string username, string password)
        {
                serviceResponse<string> response = new serviceResponse<string>();
                User user  = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));

                if(user == null)
                {
                    response.Success = false;
                    response.Message = "User not Found";

                }
            else if(!VerifyPassword(password, user.PasswordHash,user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";

            }
            else {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<serviceResponse<int>> Register(User user, string password)
        {
            serviceResponse<int> response = new serviceResponse<int>();
            if(await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User Already Exist";
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            response.Data = user.id; 
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if( await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;

                
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte [] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
    
       private bool VerifyPassword( string password, byte[] passwordHash, byte [] passwordSalt)
       {
             serviceResponse<int> response = new serviceResponse<int>();

            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

             var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != passwordHash[i])
                {
                    response.Success = false;
                    response.Message = "Password or Username is incorrect";        
                    return false;
                }
               
            }
                return true;
            }

       }
    
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
                
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }
    }

}