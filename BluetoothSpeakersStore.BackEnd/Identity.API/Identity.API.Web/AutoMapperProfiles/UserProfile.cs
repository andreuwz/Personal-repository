using AutoMapper;
using Identity.API.Domain;
using Identity.API.DTO.Request;
using Identity.API.DTO.Response;

namespace Identity.API.Web.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {

            CreateMap<User, GetUserModel>();

            CreateMap<User, CreateUserModel>()
                .ReverseMap();

            CreateMap<User, AdminEditUserModel>()
                .ReverseMap();

            CreateMap<User, PublishPrincipalUserModel>()
                .ReverseMap();

            CreateMap<RegisterUserModel, User>();

            CreateMap<EditUserModel, User>();
            CreateMap<User, PublishUpdatedUserModel>();
            CreateMap<User, GetUserModel>();
        }
    }
}
