
using AutoMapper;

namespace NZWALKS.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>().ReverseMap();

            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();

            //.ForMember(
            //dest=> dest.Id, options => options.MapFrom(src => src.Id) );
        }
    }
}
