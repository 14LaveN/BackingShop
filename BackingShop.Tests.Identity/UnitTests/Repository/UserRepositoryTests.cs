using AutoFixture;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Database.Identity.Data.Repositories;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Tests.Common.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BackingShop.Tests.Identity.UnitTests.Repository;

/// <summary>
/// Represents the <see cref="IUserRepository"/> tests class.
/// </summary>
public sealed class UserRepositoryTests
    : BaseIntegrationTest
{
    private readonly IUserRepository _userRepository;
    
    public UserRepositoryTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _userRepository = new UserRepository(DbContext);
    }

    /// <summary>
    /// Get user by specified name and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetUserByName_AssertWithOk()
    {
        Maybe<Domain.Identity.Entities.User> user = (await DbContext.Set<Domain.Identity.Entities.User>()
            .FirstOrDefaultAsync())!;

        if (user.HasValue)
        {
            var result = await _userRepository.GetByNameAsync(user.Value.UserName!);

            result.HasValue.Should().BeTrue();
            result.Value.FullName.Should().NotBeNull();
        }
    }
    
    /// <summary>
    /// Get user by specified identifier and returns OK result.
    /// </summary>
    [Fact]
    public async Task GetUserById_AssertWithOk()
    {
        Maybe<Domain.Identity.Entities.User> user = (await DbContext.Set<Domain.Identity.Entities.User>()
            .FirstOrDefaultAsync())!;

        if (user.HasValue)
        {
            var result = await _userRepository.GetByIdAsync(user.Value.Id);

            result.HasValue.Should().BeTrue();
            result.Value.FullName.Should().NotBeNull();
        }
    }
}