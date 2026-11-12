using VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using VocaFy.NextPath.Platform.Shared.Interfaces.Rest.Extensions;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Components;

public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        var allowAnonymous = context.GetEndpoint()?.Metadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute)) ?? false;

        if (!allowAnonymous)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = string.IsNullOrEmpty(token) ? null : await tokenService.ValidateToken(token);

            if (userId is not null)
            {
                var user = await userQueryService.Handle(new GetUserByIdQuery(new UserId(userId.Value)),
                    context.RequestAborted);
                if (user is { IsSessionActive: true })
                {
                    context.Items["User"] = user;
                    context.Items[AuthenticatedUserExtensions.UserIdKey] = user.Id.Value;
                }
            }
        }

        await next(context);
    }
}
