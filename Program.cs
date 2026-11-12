using VocaFy.NextPath.Platform.Iam.Application.Acl;
using VocaFy.NextPath.Platform.Iam.Application.CommandServices;
using VocaFy.NextPath.Platform.Iam.Application.Internal.CommandServices;
using VocaFy.NextPath.Platform.Iam.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.Iam.Application.Internal.QueryServices;
using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Notifications.Logging.Services;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using VocaFy.NextPath.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using VocaFy.NextPath.Platform.Iam.Interfaces.Acl;
using VocaFy.NextPath.Platform.Iam.Resources;
using VocaFy.NextPath.Platform.Resources.Errors;
using VocaFy.NextPath.Platform.Resources.Shared;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;
using VocaFy.NextPath.Platform.VocationalResources.Application.CommandServices;
using VocaFy.NextPath.Platform.VocationalResources.Application.Internal.CommandServices;
using VocaFy.NextPath.Platform.VocationalResources.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.VocationalResources.Application.Internal.QueryServices;
using VocaFy.NextPath.Platform.VocationalResources.Application.QueryServices;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Repositories;
using VocaFy.NextPath.Platform.VocationalResources.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using VocaFy.NextPath.Platform.VocationalResources.Resources;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;
using ProblemDetailsFactory = VocaFy.NextPath.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

// This application runs as a console/web process; avoid Windows Event Log writes
// that can fail under non-administrator accounts.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ---------- Web API ----------
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// ProblemDetails (RFC 9457) and global exception handler
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// CORS: allows the Vue web app (localhost:5173 and GitHub Pages) to call the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ---------- Database (MySQL + EF Core) ----------
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NoEntityTypeConfigurationsWarning))
        .EnableDetailedErrors();

});

// ---------- Localization (en / es) ----------
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<IStringLocalizer<IamMessages>, StringLocalizer<IamMessages>>();
builder.Services
    .AddSingleton<IStringLocalizer<VocationalResourcesMessages>, StringLocalizer<VocationalResourcesMessages>>();

builder.Services.AddSingleton<ProblemDetailsFactory>();

// ---------- OpenAPI / Scalar ----------
builder.Services.AddOpenApi(options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

// ---------- Dependency Injection ----------

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPsychologistRepository, PsychologistRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IPasswordResetCommandService, PasswordResetCommandService>();
builder.Services.AddScoped<IStudentCommandService, StudentCommandService>();
builder.Services.AddScoped<IPsychologistCommandService, PsychologistCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IStudentQueryService, StudentQueryService>();
builder.Services.AddScoped<IPsychologistQueryService, PsychologistQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IPasswordResetNotificationService, LoggingPasswordResetNotificationService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

// Vocational Resources Bounded Context
builder.Services.AddScoped<IVocationalResourceRepository, VocationalResourceRepository>();
builder.Services.AddScoped<IVocationalResourceCommandService, VocationalResourceCommandService>();
builder.Services.AddScoped<IVocationalResourceQueryService, VocationalResourceQueryService>();
builder.Services.AddScoped<ExternalIamService>();

// (Each teammate registers here the services of their bounded context)

// ---------- Mediator (Cortex) ----------
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(ExceptionHandlingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(TransactionCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

// Database migrations are an explicit deployment/local setup step. Enable startup
// migration only when a database and migration set are configured for this environment.
if (builder.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// ---------- HTTP pipeline ----------
app.UseExceptionHandler();

var supportedCultures = new[] { "en", "es" };
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowAllPolicy");

app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
