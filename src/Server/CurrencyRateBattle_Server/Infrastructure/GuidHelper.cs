namespace CurrencyRateBattleServer.Infrastructure;

public static class GuidHelper
{
    /// <summary>
    /// Receives a token from a session and pulls user id from it;
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/> To get a session and get a token out of there;</param>
    /// <returns>
    ///the task result contains <see cref="Guid"/> - user id;
    /// </returns>
    public static string? GetGuidFromRequest(HttpContext context)
    {
        var user = context.User;

        if (user.HasClaim(c => c.Type == "UserId"))
        {
            var id = user.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;
            return id;
        }

        return null!;
    }
}
