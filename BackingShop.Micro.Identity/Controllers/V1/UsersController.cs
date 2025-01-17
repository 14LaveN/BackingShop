using BackingShop.Application.ApiHelpers.Contracts;
using BackingShop.Application.ApiHelpers.Infrastructure;
using BackingShop.Application.ApiHelpers.Policy;
using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Database.Identity.Data.Interfaces;
using BackingShop.Domain.Common.Core.Errors;
using BackingShop.Domain.Common.Core.Primitives.Result;
using BackingShop.Domain.Common.ValueObjects;
using BackingShop.Micro.Identity.Contracts.Users.ChangeName;
using BackingShop.Micro.Identity.Contracts.Users.Login;
using BackingShop.Micro.Identity.Contracts.Users.Register;
using BackingShop.Micro.Identity.Mediatr.Commands.ChangeName;
using BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;
using BackingShop.Micro.Identity.Mediatr.Commands.Login;
using BackingShop.Micro.Identity.Mediatr.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackingShop.Micro.Identity.Controllers.V1;

/// <summary>
/// Represents the users controller class.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="userRepository">The user repository.</param>
[Route("api/v1/users")]
public sealed class UsersController(
        ISender sender,
        IUserRepository userRepository)
    : IdentityApiController(sender, userRepository)
{
    #region Commands.
    
    /// <summary>
    /// Login user.
    /// </summary>
    /// <param name="request">The <see cref="LoginRequest"/> class.</param>
    /// <returns>Base information about login user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(loginRequest => new LoginCommand(loginRequest.UserName,Password.Create(loginRequest.Password).Value))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);
    
    /// <summary>
    /// Register user.
    /// </summary>
    /// <param name="request">The <see cref="RegisterRequest"/> class.</param>
    /// <returns>Base information about register user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Register)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(registerRequest => new RegisterCommand(
                    FirstName.Create(registerRequest.FirstName).Value,
                    LastName.Create(registerRequest.LastName).Value,
                    new EmailAddress(registerRequest.Email),
                    Password.Create(registerRequest.Password).Value,
                    registerRequest.UserName))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);

    /// <summary>
    /// Change password from user.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns>Base information about change password from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.ChangePassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] string password) =>
        await Result.Create(password, DomainErrors.General.UnProcessableRequest)
            .Map(changePasswordRequest => new ChangePasswordCommand(changePasswordRequest))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)))
            .Match(Ok, BadRequest);
    
    /// <summary>
    /// Change name from user.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Base information about change name from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.ChangeName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangeName([FromBody] ChangeNameRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(changeNameRequest => new ChangeNameCommand(
                FirstName.Create(changeNameRequest.FirstName).Value,
                LastName.Create(changeNameRequest.LastName).Value))
            .Bind(async command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);
    
    #endregion
}