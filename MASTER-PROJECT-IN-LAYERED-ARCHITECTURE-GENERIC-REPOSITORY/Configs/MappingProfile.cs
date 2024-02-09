using AutoMapper;
using Master_BLL.DTOs.Authentication;
using Master_BLL.DTOs.RegistrationDTOs;
using Master_DAL.Models;

namespace MASTER_PROJECT_IN_LAYERED_ARCHITECTURE_GENERIC_REPOSITORY.Configs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationCreateDTOs, ApplicationUser>().ReverseMap();
            CreateMap<UserDTOs, ApplicationUser>().ReverseMap();
            
        }
    }
}
