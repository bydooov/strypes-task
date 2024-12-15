using AutoMapper;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Business.Mappings
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            ConfigureLocationMapping();
        }

        private void ConfigureLocationMapping()
        {
            _ = CreateMap<LocationRequestModel, Location>()
                .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Country))
                .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => src.LastUpdated))
                .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<LocationType>(src.Type!, true)));

            _ = CreateMap<Location, LocationModel>()
                .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Country))
                .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => src.LastUpdated))
                .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<PatchLocationRequestModel, Location>()
             .ForMember(dest => dest.Type, opt =>
                opt.PreCondition(src => !string.IsNullOrEmpty(src.Type) && Enum.TryParse<LocationType>(src.Type, true, out _)))
             .ForMember(dest => dest.LastUpdated, opt => opt.PreCondition(src => src.LastUpdated.HasValue))
             .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
