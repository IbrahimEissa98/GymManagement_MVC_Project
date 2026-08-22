using AutoMapper;
using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.Extensions;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;

namespace GymManagement_MVC_Project.BLL.Profiles;

public class HealthRecordDtoProfile : Profile
{
    public HealthRecordDtoProfile()
    {
        CreateMap<HealthRecordCreateDto, HealthRecord>()
            .ForMember(d => d.BloodType, s => s.MapFrom(h => TryParseEnum<BloodTypes>(h.BloodType)));

        CreateMap<Member, HealthRecordDetailsDto>()
            .ForMember(d => d.PhotoUrl, s => s.MapFrom(m => m.Photo))
            .ForMember(d => d.Height, s => s.MapFrom(m => (int)m.HealthRecord.Height))
            .ForMember(d => d.Weight, s => s.MapFrom(m => (int)m.HealthRecord.Weight))
            .ForMember(d => d.BloodType, s => s.MapFrom(m => m.HealthRecord.BloodType.GetDisplayName()))
            .ForMember(d => d.Note, s => s.MapFrom(m => m.HealthRecord.Note ?? "-"));
    }


    private static T TryParseEnum<T>(string value) where T : struct
    {
        Enum.TryParse(value, true, out T result);
        return result;
    }
}
