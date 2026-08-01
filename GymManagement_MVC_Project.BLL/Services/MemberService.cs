using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class MemberService(IMemberRepository memberRepo) : IMemberService
{
    public async Task<IReadOnlyList<MemberIndexDto>> GetAllAsync(CancellationToken ct)
    {
        var members = await memberRepo.GetAllAsync(ct);

        return [.. members.Select(m => new MemberIndexDto
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Gender = m.Gender.ToString(),
            Phone = m.Phone,
            PhotoUrl = m.Photo
        })];
    }

    public async Task<bool> CreateAsync(MemberCreateDto createDto, CancellationToken ct = default)
    {
        var email = createDto.Email.Trim().ToLower();
        if (await memberRepo.IsEmailExistAsync(email, ct))
            return false;

        var phone = createDto.Phone.Trim().ToLower();
        if (await memberRepo.IsPhoneExistAsync(createDto.Phone, ct))
            return false;

        if (!Enum.TryParse(createDto.Gender, true, out GenderTypes gender))
            return false;

        if (!Enum.TryParse(createDto.HealthRecord.BloodType, true, out BloodTypes bloodType))
            return false;

        var member = new Member
        {
            Name = createDto.Name,
            Email = email,
            Phone = phone,
            DateOfBirth = createDto.DateOfBirth,
            Gender = gender,
            Address = new Address
            {
                City = createDto.City,
                Street = createDto.Street,
                BuildingNumber = createDto.BuildingNumber,
            },
            HealthRecord = new HealthRecord
            {
                Height = createDto.HealthRecord.Height,
                Weight = createDto.HealthRecord.Weight,
                Note = createDto.HealthRecord.Note,
                BloodType = bloodType
            },
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await memberRepo.AddAsync(member, ct);

        var result = await memberRepo.SaveChangesAsync(ct);

        if (result == 0) return false;

        return true;
    }
}
