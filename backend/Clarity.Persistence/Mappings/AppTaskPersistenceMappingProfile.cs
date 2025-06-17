using AutoMapper;
using Clarity.Core.Models;
using Clarity.Persistence.Entitites;

namespace Clarity.Persistence.Mappings;

public class AppTaskPersistenceMappingProfile : Profile
{
    public AppTaskPersistenceMappingProfile()
    {
        CreateMap<AppTaskEntity, AppTask>();
        CreateMap<TagEntity, Tag>();
    }
}