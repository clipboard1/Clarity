using AutoMapper;
using Clarity.Core.Models;
using Clarity.Persistence.Entitites;

namespace Clarity.Persistence.Mappings;

public class TagPersistenceMappingProfile : Profile
{
    public TagPersistenceMappingProfile()
    {
        CreateMap<TagEntity, Tag>();
    }
}