using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService
    {
        private readonly AccountRepository _repo;

        public AccountService()
        {
            _repo = new AccountRepository();
        }

        public async Task<SystemAccount> Login(string email, string password)
        {
            if (email == "admin@FUNewsManagementSystem.org" && password == "@@abc123@@") return new SystemAccount
            {
                AccountEmail = email,
                AccountId = 0,
                AccountName = "Admin",
                AccountPassword = password,
                AccountRole = 3
            };
            return await _repo.Login(email, password);
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
            if (await _repo.CheckExistEmail(Email) != null)
            {
                return false;
            }

            var l = await _repo.GetAllAsync();

            await _repo.CreateAsync(new SystemAccount
            {
                AccountEmail = Email,
                AccountName = Fullname,
                AccountPassword = Password,
                AccountRole = Role,
                AccountId = (short)(l.Count + 1)
            });
            return true;
        }

        public async Task<bool> RemoveAccount(short id)
        {
            var user = await _repo.GetAccountById(id);

            if (user == null)
            {
                return false;
            }

            if(user.NewsArticles.Any())
            {
                return false;
            }

            await _repo.RemoveAsync(user);
            return true;
        }

        public async Task<bool> UpdateAccount(short id, string? Fullname, string? Email, string? Password, int? Role)
        {
            var user = await _repo.GetAccountById(id);

            if (user == null)
            {
                return false;
            }

            var existingEmail = await _repo.CheckExistEmail(Email);

            if( existingEmail != null && existingEmail.AccountId != user.AccountId)
            {
                return false;
            }
            
            if(Fullname != null) user.AccountName = Fullname;
            if (Password != null) user.AccountPassword = Password;
            if (Role != null) user.AccountRole = Role;
            if (Email != null) user.AccountEmail = Email;

            await _repo.UpdateAsync(user);
            return true;
        }
    }
}
