using Clarity.Api.Mappings;
using Clarity.Api.Validators.AppTasks;
using Clarity.Api.Validators.Tags;
using Clarity.Api.Validators.Users;
using Clarity.Application;
using Clarity.Application.Abstractions;
using Clarity.Application.AppTasks.Create;
using Clarity.Application.AppTasks.Delete;
using Clarity.Application.AppTasks.GetAll;
using Clarity.Application.AppTasks.GetById;
using Clarity.Application.AppTasks.Update;
using Clarity.Application.Tags.Create;
using Clarity.Application.Tags.Delete;
using Clarity.Application.Tags.GetAllByTask;
using Clarity.Application.Users.Login;
using Clarity.Application.Users.LoginByRefreshToken;
using Clarity.Application.Users.Logout;
using Clarity.Application.Users.LogoutAllDevices;
using Clarity.Application.Users.Register;
using Clarity.Application.Validators.AppTasks;
using Clarity.Application.Validators.Tags;
using Clarity.Application.Validators.User;
using Clarity.Core.Abstractions;
using Clarity.Core.Dto;
using Clarity.Core.Models;
using Clarity.Infrastructure.Authentication;
using Clarity.Persistence.Abstractions;
using Clarity.Persistence.Mappings;
using Clarity.Persistence.Repositories;
using FluentValidation;

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
        services.AddScoped<ITagRepository, TagRepository>();
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
    
    public static void AddAppTaskCommandsAndQueries(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateTaskCommand, Guid>, CreateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTaskCommand>, UpdateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTaskCommand>, DeleteTaskCommandHandler>();
        services.AddScoped<IQueryHandler<GetAllTasksQuery, List<AppTask>>, GetAllTasksQueryHandler>();
        services.AddScoped<IQueryHandler<GetTaskByIdQuery, AppTask>, GetTaskByIdQueryHandler>();
    }


    public static void AddTagCommandsAndQueries(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateTagCommand, int>, CreateTagCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTagCommand>, DeleteTagCommandHandler>();
        services.AddScoped<IQueryHandler<GetAllTagsByTaskQuery, List<Tag>>, GetAllTagsByTaskQueryHandler>();
    }
    
    public static void AddAutoMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AppTaskApiMappingProfile));
        services.AddAutoMapper(typeof(AppTaskPersistenceMappingProfile));
    }

    public static void AddApiValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AppTaskCreateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AppTaskUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
        
        services.AddValidatorsFromAssemblyContaining<TagCreateRequestValidator>();
    }
    
    public static void AddAppValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteTaskCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateTaskCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetAllTasksQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetTaskByIdQueryValidator>();
        
        services.AddValidatorsFromAssemblyContaining<LoginByRefreshTokenCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<LogoutAllDevicesCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<LogoutCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
        
        services.AddValidatorsFromAssemblyContaining<CreateTagCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteTagCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetAllTagsByTaskQueryValidator>();
    }
}