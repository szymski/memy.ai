using System.Security.Claims;
using Domain.Auth.ValueObjects;

namespace WebApi.Auth.Services;

public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) {
    private CurrentUserData? _user = null;

    /// <summary>
    /// Returns whether the request is authenticated and current user data can be accessed.
    /// </summary>
    public bool IsAuthenticated => httpContextAccessor.HttpContext!.User.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// Gets authenticated user data from HTTP context.
    /// Should only be called if <see cref="IsAuthenticated"/> returns true.
    /// <exception cref="InvalidCastException">Throws if HTTP request is not authenticated.</exception>
    /// </summary>
    public CurrentUserData User => _user ??= TryBuildFromContext();

    private CurrentUserData TryBuildFromContext()
    {
        if (!IsAuthenticated)
            throw new InvalidOperationException("Cannot get current user data from HTTP context, because the request isn't authenticated.");
        return From(httpContextAccessor.HttpContext!.User);
    }

    public static CurrentUserData From(ClaimsPrincipal principal)
    {
        var claimId = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier);
        var claimEmail = principal.Claims.Single(c => c.Type == ClaimTypes.Name);
        return new(int.Parse(claimId.Value), claimEmail.Value);
    }
}