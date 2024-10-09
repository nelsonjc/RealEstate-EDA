using AutoMapper;
using RealEstate.Database.Entities;
using RealEstate.Shared.CustomEntities;
using RealEstate.Shared.Utils;

namespace RealEstate.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Property, PropertyReport>()
                .ForMember(x => x.Id, o => o.MapFrom(s => Guid.NewGuid()));

            CreateMap<Owner, OwnerReport>()
                .ForMember(x => x.Id, o => o.MapFrom(s => Guid.NewGuid()));

            CreateMap<PropertyImage, PropertyImageReport>()
                .ForMember(x => x.Id, o => o.MapFrom(s => Guid.NewGuid()))
                .ForMember(x => x.FileUrl, o => o.MapFrom(s => FileUtil.SaveImage("Property", s.IdProperty.ToString(), s.FileBase64)));

            CreateMap<PropertyTrace, PropertyTraceReport>()
                .ForMember(x => x.Id, o => o.MapFrom(s => Guid.NewGuid()));
        }
    }
}
