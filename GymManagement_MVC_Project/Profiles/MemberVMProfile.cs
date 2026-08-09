using AutoMapper;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.PL.ViewModels.Member;

namespace GymManagement_MVC_Project.PL.Profiles;

public class MemberVMProfile : Profile
{
    public MemberVMProfile()
    {
        CreateMap<MemberIndexViewModel, MemberIndexDto>().ReverseMap();

        CreateMap<MemberCreateViewModel, MemberCreateDto>();

        CreateMap<MemberDetailsDto, MemberDetailsViewModel>();

        CreateMap<MemberToUpdateViewModel, MemberToUpdateDto>().ReverseMap();
    }
}
