using AutoMapper;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Business.Mappings
{
    public class ChargePointProfile : Profile
    {
        public ChargePointProfile()
        {
            ConfigureChargePointMapping();
        }

        private void ConfigureChargePointMapping()
        {
            _ = CreateMap<ChargePointModel, ChargePoint>()
               .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => src.LastUpdated))
               .ForMember(dest => dest.ChargePointId, opts => opts.MapFrom(src => src.ChargePointId))
               .ForMember(dest => dest.FloorLevel, opts => opts.MapFrom(src => src.FloorLevel))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ChargePointStatus>(src.Status!, true)));

            _ = CreateMap<ChargePoint, ChargePointModel>()
               .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => src.LastUpdated))
               .ForMember(dest => dest.ChargePointId, opts => opts.MapFrom(src => src.ChargePointId))
               .ForMember(dest => dest.FloorLevel, opts => opts.MapFrom(src => src.FloorLevel))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
