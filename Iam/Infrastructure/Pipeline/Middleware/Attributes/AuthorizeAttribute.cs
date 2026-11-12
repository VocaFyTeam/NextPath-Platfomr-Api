using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly EUserRole[] _allowedRoles;

    public AuthorizeAttribute(params EUserRole[] allowedRoles) => _allowedRoles = allowedRoles;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous)
        {
            Console.WriteLine(" Skipping authorization");
            return;
        }

        var user = (User?)context.HttpContext.Items["User"];

        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(user.Role))
            context.Result = new ForbidResult();
    }
}
