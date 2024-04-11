using BackingShop.Database.Common;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using BackingShop.Domain.Entities;
using BackingShop.Domain.Identity.Entities;
using IUserRepository = BackingShop.Database.Identity.Data.Interfaces.IUserRepository;

namespace BackingShop.Database.Identity.Data.Repositories;

public class UserRepository(BaseDbContext userDbContext)
    : IUserRepository
{
    public async Task<Maybe<User>> GetByIdAsync(Guid userId) =>
            await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.Id == userId) 
            ?? throw new ArgumentNullException();

    public async Task<Maybe<User>> GetByNameAsync(string name) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.UserName == name) 
        ?? throw new ArgumentNullException();

    public async Task<Maybe<User>> GetByEmailAsync(EmailAddress emailAddress) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.EmailAddress == emailAddress) 
        ?? throw new ArgumentNullException();
}