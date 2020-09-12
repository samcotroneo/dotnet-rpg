using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using dotnet_rpg.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;
        }

        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            // Attempt to login with the given credentials, password authentication goes here.
            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            if(await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            // Generate a hash/salt for the specified password.
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Store the password hash and salt in the user object.
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Register created user with password
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            response.Data = user.Id;

            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            // As the method simple returns a boolean, there is no need for a service response.
            // Check and see if the user exists (case insensitive).
            if (await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Hash the password using the HMACSHA512 algorithm, used the in built key as salt.
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}