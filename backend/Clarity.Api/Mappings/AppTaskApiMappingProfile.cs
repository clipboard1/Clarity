using AutoMapper;
using Clarity.Api.Contracts.AppTasks;
using Clarity.Core.Models;

namespace Clarity.Api.Mappings;

public class AppTaskApiMappingProfile : Profile
{
    public AppTaskApiMappingProfile()
    {
        CreateMap<AppTask, AppTaskResponse>();
    }
}