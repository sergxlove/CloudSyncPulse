using CloudSyncPulse.DataAccess.Sqlite.Repositories;
using CloudSyncPulse.Web.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CloudSyncPulse.Web.Endpoints
{
    public static class ProvidersEndpoints
    {
        public static IEndpointRouteBuilder MapProvidersEndpoints(this IEndpointRouteBuilder app)
        {
            // ---------- Список ----------
            app.MapGet("/api/providers", async (
                [FromServices] ProvidersRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var r = await repo.GetAllAsync(token);
                    if (!r.IsSuccess) return Results.BadRequest(r.Error);
                    return Results.Ok(r.Value);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            // ---------- По id ----------
            app.MapGet("/api/providers/{id:guid}", async (Guid id,
                [FromServices] ProvidersRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var r = await repo.GetByIdAsync(id, token);
                    if (!r.IsSuccess) return Results.BadRequest(r.Error);
                    if (r.Value is null) return Results.NotFound();
                    return Results.Ok(r.Value);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            // ---------- Создать ----------
            app.MapPost("/api/providers", async (
                CreateProviderRequest request,
                [FromServices] ProvidersRepository providersRepo,
                [FromServices] ProviderCredentialsRepository credentialsRepo,
                [FromServices] ICredentialProtector protector,
                CancellationToken token) =>
            {
                try
                {
                    var created = Providers.Create(request.Type, request.DisplayName, request.IsEnabled);
                    if (!created.IsSuccess)
                        return Results.BadRequest(created.Error);

                    var addRes = await providersRepo.AddAsync(created.Value!, token);
                    if (!addRes.IsSuccess)
                        return Results.BadRequest(addRes.Error);

                    if (!string.IsNullOrWhiteSpace(request.AccessToken))
                    {
                        var accessEnc = protector.Protect(request.AccessToken);
                        var refreshEnc = string.IsNullOrWhiteSpace(request.RefreshToken)
                            ? null
                            : protector.Protect(request.RefreshToken);

                        var cred = ProviderCredential.Create(
                            created.Value!.Id, accessEnc, refreshEnc,
                            request.ExpiresAtUtc, request.Scope, request.TokenType);

                        if (!cred.IsSuccess)
                            return Results.BadRequest(cred.Error);

                        var credRes = await credentialsRepo.AddAsync(cred.Value!, token);
                        if (!credRes.IsSuccess)
                            return Results.BadRequest(credRes.Error);
                    }

                    return Results.Ok(created.Value!.Id);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            // ---------- Переименовать ----------
            app.MapPut("/api/providers/{id:guid}/rename", async (Guid id,
                RenameProviderRequest request,
                [FromServices] ProvidersRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var found = await repo.GetByIdAsync(id, token);
                    if (!found.IsSuccess) return Results.BadRequest(found.Error);
                    if (found.Value is null) return Results.NotFound();

                    found.Value.Rename(request.DisplayName);
                    var upd = await repo.UpdateAsync(found.Value, token);
                    if (!upd.IsSuccess) return Results.BadRequest(upd.Error);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            // ---------- Вкл/выкл ----------
            app.MapPut("/api/providers/{id:guid}/enabled", async (Guid id,
                SetEnabledRequest request,
                [FromServices] ProvidersRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var found = await repo.GetByIdAsync(id, token);
                    if (!found.IsSuccess) return Results.BadRequest(found.Error);
                    if (found.Value is null) return Results.NotFound();

                    if (request.Enabled) found.Value.Enable();
                    else found.Value.Disable();

                    var upd = await repo.UpdateAsync(found.Value, token);
                    if (!upd.IsSuccess) return Results.BadRequest(upd.Error);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            // ---------- Удалить ----------
            app.MapDelete("/api/providers/{id:guid}", async (Guid id,
                [FromServices] ProvidersRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var r = await repo.DeleteAsync(id, token);
                    if (!r.IsSuccess) return Results.BadRequest(r.Error);
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            }).RequireAuthorization("OnlyForAuthUser")
              .RequireRateLimiting("GeneralPolicy");

            // ---------- Проверить подключение ----------
            // Пока нет реальных ICloudProvider-реализаций — проверяем только
            // наличие кредов и срок их действия.
            app.MapPost("/api/providers/{id:guid}/check", async (Guid id,
                [FromServices] ProvidersRepository providersRepo,
                [FromServices] ProviderCredentialsRepository credentialsRepo,
                [FromServices] ICredentialProtector protector,
                CancellationToken token) =>
            {
                try
                {
                    var found = await providersRepo.GetByIdAsync(id, token);
                    if (!found.IsSuccess) return Results.BadRequest(found.Error);
                    if (found.Value is null) return Results.NotFound();

                    var credRes = await credentialsRepo.GetByProviderIdAsync(id, token);
                    if (!credRes.IsSuccess) return Results.BadRequest(credRes.Error);
                    if (credRes.Value is null)
                        return Results.BadRequest("У провайдера нет сохранённых токенов.");

                    if (credRes.Value.AccessTokenEncrypted is null ||
                        credRes.Value.AccessTokenEncrypted.Length == 0)
                        return Results.BadRequest("Access-токен пуст.");

                    if (credRes.Value.ExpiresAtUtc.HasValue &&
                        credRes.Value.ExpiresAtUtc.Value <= DateTime.UtcNow)
                        return Results.BadRequest("Access-токен истёк.");

                    return Results.Ok(new { status = "ok" });
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
