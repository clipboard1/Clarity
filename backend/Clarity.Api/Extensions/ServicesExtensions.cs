using Clarity.Api.Mappings;
using Clarity.Application;
using Clarity.Application.Abstractions;
using Clarity.Application.AppTasks.Create;
using Clarity.Application.AppTasks.Delete;
using Clarity.Application.AppTasks.GetAll;
using Clarity.Application.AppTasks.GetById;
using Clarity.Application.AppTasks.Update;
using Clarity.Application.LogoutAllDevices;
using Clarity.Application.Users.Login;
using Clarity.Application.Users.LoginByRefreshToken;
using Clarity.Application.Users.Logout;
using Clarity.Application.Users.Register;
using Clarity.Core.Abstractions;
using Clarity.Core.Dto;
using Clarity.Core.Models;
using Clarity.Infrastructure.Authentication;
using Clarity.Persistence.Abstractions;
using Clarity.Persistence.Mappings;
using Clarity.Persistence.Repositories;

namespace Clarity.Api.Extensions;

public static class ServicesExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
    }
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IAppTasksRepository, AppTasksRepository>();
    }

    public static void AddDispatchers(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
    
    public static void AddAuthCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
        services.AddScoped<ICommandHandler<LoginCommand, AuthTokens>, LoginCommandHandler>();
        services.AddScoped<ICommandHandler<LogoutCommand>, LogoutCommandHandler>();
        services.AddScoped<ICommandHandler<LogoutAllDevicesCommand>, LogoutAllDevicesCommandHandler>();
        services.AddScoped<ICommandHandler<LoginByRefreshTokenCommand, AuthTokens>,LoginByRefreshTokenCommandHandler>();
    }
    
    public static void AddAppTaskCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateTaskCommand, Guid>, CreateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTaskCommand>, UpdateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTaskCommand>, DeleteTaskCommandHandler>();
    }
    
    public static void AddAppTaskQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetAllTasksQuery, List<AppTask>>, GetAllTasksQueryHandler>();
        services.AddScoped<IQueryHandler<GetTaskByIdQuery, AppTask>, GetTaskByIdQueryHandler>();
    }

    public static void AddAutoMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AppTaskApiMappingProfile));
        services.AddAutoMapper(typeof(AppTaskPersistenceMappingProfile));
    }
}