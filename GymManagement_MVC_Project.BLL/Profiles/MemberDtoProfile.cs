using AutoMapper;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;

namespace GymManagement_MVC_Project.BLL.Profiles;

public class MemberDtoProfile : Profile
{
    public MemberDtoProfile()
    {
        CreateMap<Member, MemberIndexDto>()
            .ForMember(d => d.PhotoUrl, s => s.MapFrom(m => m.Photo))
            .ForMember(d => d.Gender, s => s.MapFrom(m => m.Gender.ToString()));

        CreateMap<MemberCreateDto, Member>()
            .ForMember(d => d.Email, s => s.MapFrom(m => m.Email.Trim().ToLowerInvariant()))
            .ForMember(d => d.Gender, s => s.MapFrom(m => TryParseEnum<GenderTypes>(m.Gender)))
            .ForMember(d => d.Address, s => s.MapFrom(m => new Address
            {
                BuildingNumber = m.BuildingNumber,
                Street = m.Street,
                City = m.City
            }))
            .ForMember(d => d.JoinDate, s => s.MapFrom(m => DateOnly.FromDateTime(DateTime.Now)));

        CreateMap<Member, MemberDetailsDto>()
            .ForMember(d => d.PhotoUrl, s => s.MapFrom(s => s.Photo))
            .ForMember(d => d.Gender, s => s.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DateOfBirth, s => s.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.Address, s => s.MapFrom(s => $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"))
            .ForMember(d => d.PlanName, s => s.MapFrom(m => ResolvePlanName(m.Memberships.FirstOrDefault())))
            .ForMember(d => d.MembershipStartDate, s => s.MapFrom(m => ResolveMembershipStartDate(m.Memberships.FirstOrDefault())))
            .ForMember(d => d.MembershipEndDate, s => s.MapFrom(m => ResolveMembershipEndDate(m.Memberships.FirstOrDefault())));

        CreateMap<Member, MemberToUpdateDto>()
            .ForMember(d => d.PhotoUrl, s => s.MapFrom(m => m.Photo))
            .ForMember(d => d.BuildingNumber, s => s.MapFrom(m => m.Address.BuildingNumber))
            .ForMember(d => d.Street, s => s.MapFrom(m => m.Address.Street))
            .ForMember(d => d.City, s => s.MapFrom(m => m.Address.City))
            .ReverseMap();

        CreateMap<Member, MemberDeleteDto>();
    }

    private static string ResolveMembershipEndDate(Membership? membership)
    {
        return membership?.EndDate.ToShortDateString() ?? "-";
    }

    private static string ResolveMembershipStartDate(Membership? membership)
    {
        return membership?.StartDate.ToShortDateString() ?? "-";
    }

    private string ResolvePlanName(Membership? membership)
    {
        return membership?.Plan.Name ?? "No Active Plan";
    }

    private static T TryParseEnum<T>(string value) where T : struct
    {
        Enum.TryParse(value, true, out T result);
        return result;
    }
}
