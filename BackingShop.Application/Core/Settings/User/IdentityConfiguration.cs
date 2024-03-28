using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace BackingShop.Application.Core.Settings.User;

/// <summary>
/// Represents the identity configuration class.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// The api scopes list.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("BackingShop.Micro.Identity", "Identity")
        };

    /// <summary>
    /// The identity resources list.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    /// <summary>
    /// The api resources list.
    /// </summary>
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("BackingShop.Micro.Identity", "Identity", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"BackingShop.Micro.Identity"}
            }
        };

    /// <summary>
    /// The clients list.
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "BackingShop-Identity",
                ClientName = "BackingShop.Micro.Identity",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:44460/signin"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:44460"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:44460/signout"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ProgrammerAnswers.Micro.ImageAPI"
                },
                AllowAccessTokensViaBrowser = true
            }
        };
}