using AutoMapper;
using Dal.Impl.Entities;
using SecurePrivacy.Sample.Model;

namespace Dal.Impl.MappingProfiles
{
    public class StuffMappingProfile : Profile
    {
        /// <summary>
        /// Gets the profile name
        /// </summary>
        public override string ProfileName => nameof(StuffMappingProfile);

        /// <summary>
        /// Constructor
        /// </summary>
        public StuffMappingProfile()
        {
            CreateMap<Stuff, StuffEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
}
