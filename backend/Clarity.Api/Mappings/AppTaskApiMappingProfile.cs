using AutoMapper;
using Clarity.Api.Contracts.AppTasks;
using Clarity.Api.Contracts.Tags;
using Clarity.Core.Models;

namespace Clarity.Api.Mappings;

public class AppTaskApiMappingProfile : Profile
{
    public AppTaskApiMappingProfile()
    {
        CreateMap<AppTask, AppTaskResponse>();
        CreateMap<Tag, TagResponse>();
    }
}