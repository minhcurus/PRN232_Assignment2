using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{
    public class AccountRepository : GenericRepository<SystemAccount>
    {
        public AccountRepository() { }

        public async Task<SystemAccount> Login(string email, string password) 
            => await _context.SystemAccounts.Where(u => (u.AccountEmail.Equals(email)) && (u.AccountPassword.Equals(password))).FirstOrDefaultAsync();
        
        public async Task<SystemAccount> CheckExistEmail(string email)
            => await _context.SystemAccounts.Where(u => u.AccountEmail.Equals(email)).FirstOrDefaultAsync();

        public async Task<SystemAccount> GetAccountById(short id)
            => await _context.SystemAccounts.Include(u => u.NewsArticles).Where(u => u.AccountId.Equals(id)).FirstOrDefaultAsync();
    }
}
