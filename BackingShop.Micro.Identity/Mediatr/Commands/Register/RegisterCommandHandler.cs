using System.Security.Authentication;
using BackingShop.Application.ApiHelpers.Responses;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.Application.Core.Settings.User;
using BackingShop.Domain.Common.Core.Exceptions;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.Enumerations;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Domain.Identity.Entities;
using BackingShop.Micro.Identity.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BackingShop.Micro.Identity.Mediatr.Commands.Register;

/// <summary>
/// Represents the register command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="userManager">The user manager.</param>
/// <param name="sender">The sender.</param>
/// <param name="signInManager">The sign in manager.</param>
/// <param name="jwtOptions">The json web token options.</param>
internal sealed class RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger,
        UserManager<User> userManager,
        ISender sender,
        SignInManager<User> signInManager,
        IOptions<JwtOptions> jwtOptions)
    : ICommandHandler<RegisterCommand, LoginResponse<Result>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException();
 
    /// <inheritdoc />
    public async Task<LoginResponse<Result>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for login an account - {request.UserName} {request.LastName}");
            
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);
            Result<EmailAddress> emailResult = EmailAddress.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);
            
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is not null)
            {
                logger.LogWarning("User with the same name already taken");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            user = User.Create(firstNameResult.Value, lastNameResult.Value,request.UserName, emailResult.Value, passwordResult.Value);
            
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                
                logger.LogInformation($"User authorized - {user.UserName} {DateTime.UtcNow}");
            }
            
            var (refreshToken, refreshTokenExpireAt) = user.GenerateRefreshToken(_jwtOptions);
            
            if (result.Succeeded)
            {
                user.RefreshToken = refreshToken;
            }
            
            return new LoginResponse<Result>
            {
                Description = "Register account",
                StatusCode = StatusCode.Ok,
                Data = Result.Success(),
                AccessToken = user.GenerateAccessToken(_jwtOptions),
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = refreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[RegisterCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}