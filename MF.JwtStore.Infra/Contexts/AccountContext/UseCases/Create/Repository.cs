using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using MF.JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace MF.JwtStore.Infra.Contexts.AccountContext.UseCases.Create;

public class Repository : IRepository
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
        => _context = context;

    public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken)
        => await _context.Users.AsNoTracking()
                               .AnyAsync(u => u.Email.Address
                               .Equals(email), cancellationToken);

    public async Task SaveAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}