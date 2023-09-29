using System.Security.Claims;

namespace MF.JwtStore.Api.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string Id(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(
                c => c.Type.Equals("Id"))?.Value ??
                    string.Empty;

    public static string Name(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(
                c => c.Type.Equals(ClaimTypes.GivenName))?.Value ??
                    string.Empty;

    public static string Email(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(
                c => c.Type.Equals(ClaimTypes.Name))?.Value ?? 
                    string.Empty;
}