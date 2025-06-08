using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService
    {
        private readonly AccountRepository _repo;
        private readonly IConfiguration _config;

        public AccountService(IConfiguration config)
        {
            _repo = new AccountRepository();
            _config = config;

        }

        public async Task<(SystemAccount? user, string? token)> Login(string email, string password)
        {
            var user = await _repo.Login(email, password);

            if (user == null)
            {
                var adminEmail = _config["DefaultAdmin:Email"];
                var adminPass = _config["DefaultAdmin:Password"];

                if (email == adminEmail && password == adminPass)
                {
                    user = new SystemAccount
                    {
                        AccountId = 0,
                        AccountEmail = email,
                        AccountName = "Admin",
                        AccountPassword = password,
                        AccountRole = 0
                    };
                }
            }

            if (user == null) return (null, null);

            // Tạo token
            string tokenStr = GenerateToken(user);

            return (user, tokenStr);
        }

        private string GenerateToken(SystemAccount user)
        {
            var claims = new[]
                        {
            new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
            new Claim(ClaimTypes.Email, user.AccountEmail),
            new Claim(ClaimTypes.Role, user.AccountRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }

        public async Task<SystemAccount> GetAccountById(short id)
        {
            return await _repo.GetAccountById(id);
        }
        public async Task<List<SystemAccount>> GetAccounts()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<bool> CreateAccount(string Fullname, string Email, string Password, int Role)
        {
            if (await _repo.CheckExistEmail(Email) != null) return false;

            var maxId = (await _repo.GetAllAsync()).Max(a => (int?)a.AccountId) ?? 0;

            await _repo.CreateAsync(new SystemAccount
            {
                AccountEmail = Email,
                AccountName = Fullname,
                AccountPassword = Password,
                AccountRole = Role,
                AccountId = (short)(maxId + 1)
            });
            return true;
        }

        public async Task<bool> RemoveAccount(short id)
        {
            var user = await _repo.GetAccountById(id);

            if (user == null) return false;

            if (user.NewsArticles != null && user.NewsArticles.Any()) return false;

            await _repo.RemoveAsync(user);
            return true;
        }

        public async Task<bool> UpdateAccount(short id, string? Fullname, string? Email, string? Password, int? Role)
        {
            var user = await _repo.GetAccountById(id);

            if (user == null) return false;


            if (!string.IsNullOrEmpty(Email))
            {
                var existingEmail = await _repo.CheckExistEmail(Email);
                if (existingEmail != null && existingEmail.AccountId != user.AccountId) return false;
                user.AccountEmail = Email;
            }

            if (Fullname != null) user.AccountName = Fullname;
            if (Password != null) user.AccountPassword = Password;
            if (Role != null) user.AccountRole = Role.Value;

            await _repo.UpdateAsync(user);
            return true;
        }
    }
}
