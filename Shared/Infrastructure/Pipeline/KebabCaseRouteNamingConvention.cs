
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Pipeline;

public class KebabCaseRouteNamingConvention
    : RouteTokenTransformerConvention
{
    public KebabCaseRouteNamingConvention()
        : base(new KebabCaseParameterTransformer())
    {
    }
}

public class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null)
            return null;

        return Regex.Replace(
            value.ToString()!,
            "([a-z0-9])([A-Z])",
            "$1-$2"
        ).ToLowerInvariant();
    }
}
