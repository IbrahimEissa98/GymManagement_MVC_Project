using AutoMapper;
using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.PL.ViewModels.HealthRecord;

namespace GymManagement_MVC_Project.PL.Profiles;

public class HealthRecordVMProfile : Profile
{
    public HealthRecordVMProfile()
    {
        CreateMap<HealthRecordCreateViewModel, HealthRecordCreateDto>();

        CreateMap<HealthRecordDetailsDto, HealthRecordDetailsViewModel>();
    }
}
