using AutoMapper;
using UserAPI.Model;
using UserAPI.Model.Db;

namespace UserAPI.Model
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(y => y.Email))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(y => y.Password))
                .ForMember(dest => dest.Salt, opts => opts.Ignore())
                .ForMember(dest => dest.RoleId, opts => opts.MapFrom(y => y.Role))
                .ReverseMap();
        }

        /*                
        */

    }
}
