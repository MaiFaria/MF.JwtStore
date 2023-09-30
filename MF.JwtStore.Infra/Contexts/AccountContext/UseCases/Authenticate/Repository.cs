using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MF.JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace MF.JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
            => _context = context;

        public async Task<User?> GetUserByEmailAsync(string email,
                                                     CancellationToken cancellationToken)
            => await _context.Users
                             .AsNoTracking()
                             .Include(x => x.Roles)
                             .FirstOrDefaultAsync(x => x.Email.Address.Equals(email),
                                                  cancellationToken);
    }
}

