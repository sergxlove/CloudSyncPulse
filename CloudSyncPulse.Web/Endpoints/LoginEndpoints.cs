using CloudSyncPulse.Core.Abstractions;
using CloudSyncPulse.Core.Requests;
using CloudSyncPulse.DataAccess.Sqlite.Abstractions;
using CloudSyncPulse.DataAccess.Sqlite.Repositories;
using CloudSyncPulse.Web.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CloudSyncPulse.Web.Endpoints
{
    public static class LoginEndpoints
    {
        public static IEndpointRouteBuilder MapLoginEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/login/login", async (HttpContext context,
                LoginRequest request,
                [FromServices] UsersRepository usersRepo,
                [FromServices] IPasswordHasherService hasher,
                [FromServices] IJwtProviderService jwtService,
                [FromServices] IConfiguration configuration,
                CancellationToken token) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Login) ||
                        string.IsNullOrWhiteSpace(request.Password))
                        return Results.BadRequest("Пустые значения логин или пароль");

                    var userRes = await usersRepo.GetByUsernameAsync(request.Login, token);
                    if (!userRes.IsSuccess) return Results.BadRequest(userRes.Error);
                    if (userRes.Value is null) return Results.BadRequest("Неверный логин или пароль");

                    if (!hasher.VerifyBCrypt(request.Password, userRes.Value.PasswordHash))
                        return Results.BadRequest("Неверный логин или пароль");

                    var jwtSettings = configuration.GetSection("JwtSettings");
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, userRes.Value.Username),
                        new(ClaimTypes.NameIdentifier, userRes.Value.Id.ToString())
                    };

                    var jwt = jwtService.GenerateToken(new JwtRequest
                    {
                        Audience = jwtSettings["Audience"]!,
                        Issuer = jwtSettings["Issuer"]!,
                        Claims = claims,
                        SecretKey = jwtSettings["SecretKey"]!,
                        Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["Lifetime"]!))
                    });

                    if (string.IsNullOrEmpty(jwt))
                        return Results.InternalServerError();

                    context.Response.Cookies.Append("jwt", jwt);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireRateLimiting("LoginPolicy");

            app.MapGet("/api/login/logout", (HttpContext context) =>
            {
                try
                {
                    context.Response.Cookies.Delete("jwt");
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            return app;
        }
    }
}
