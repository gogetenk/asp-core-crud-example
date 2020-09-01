using AutoMapper;
using SecurePrivacy.Sample.Dto;
using SecurePrivacy.Sample.Model;

namespace SecurePrivacy.Sample.Bll.Impl.MappingProfiles
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
            CreateMap<Stuff, StuffDto>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
