using MediatR;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create;
using MF.JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using MF.JwtStore.Infra.Contexts.AccountContext.UseCases.Create;

namespace MF.JwtStore.Api.Extensions;

public static class AccountContextExtension
{
    public static void AddAccountContext(this WebApplicationBuilder builder)
    {
        #region Create

        builder.Services.AddTransient<IService, Service>();
        builder.Services.AddTransient<IRepository, Repository>();

        #endregion
    }

    public static void MapAccountEndpoints(this WebApplication app)
    {
        #region Create

        app.MapPost("api/v1/users",
                    async (Request request,
                           IRequestHandler<Request, Response> handler) =>
        {
            var result = await handler.Handle(request, new CancellationToken());

            return result.IsSuccess
                ? Results.Created($"api/v1/users/{result.Data?.Id}", result)
                : Results.Json(result, statusCode: result.Status);
        });

        #endregion
    }
}

