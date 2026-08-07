using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Extensions;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class MemberService(
    IMemberRepository memberRepo
    ) : IMemberService
{
    public async Task<IReadOnlyList<MemberIndexDto>> GetAllAsync(CancellationToken ct)
    {
        var members = await memberRepo.GetAllIncludingDeletedAsync(ct);

        return [.. members.Select(m => new MemberIndexDto
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Gender = m.Gender.ToString(),
            Phone = m.Phone,
            PhotoUrl = m.Photo,
            IsDeleted = m.IsDeleted
        })];
    }

    public async Task<bool> CreateAsync(MemberCreateDto createDto, CancellationToken ct = default)
    {
        var email = createDto.Email.Trim().ToLower();
        if (await memberRepo.IsEmailTakenAsync(email, ct: ct))
            return false;

        var phone = createDto.Phone.Trim().ToLower();
        if (await memberRepo.IsPhoneTakenAsync(createDto.Phone, ct: ct))
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

    public async Task<MemberDetailsDto?> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdWithMembershipAsync(id, ct);

        if (member is null) return null;

        var now = DateTime.UtcNow;

        var membership = member.Memberships?.Where(m => m.StartDate <= now && m.EndDate >= now)?.FirstOrDefault();

        var address = string.Join(" - ", member.Address.BuildingNumber, member.Address.Street, member.Address.City);

        var memberDto = new MemberDetailsDto
        {
            Id = member.Id,
            PhotoUrl = member.Photo,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            Gender = member.Gender.ToString(),
            DateOfBirth = member.DateOfBirth.ToString("dd/MM/yyyy"),
            Address = address,
            MembershipStartDate = membership?.StartDate.ToString("dd/MM/yyyy"),
            MembershipEndDate = membership?.EndDate.ToString("dd/MM/yyyy"),
            PlanName = membership?.Plan.Name
        };

        return memberDto;
    }

    public async Task<HealthRecordDetailsDto?> GetHealthRecordAsync(int id, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdWithIncludesAsync(id, ct: ct, includes: m => m.HealthRecord);

        if (member is null) return null;

        var healthDto = new HealthRecordDetailsDto
        {
            PhotoUrl = member.Photo,
            Name = member.Name,
            Height = (int)member.HealthRecord.Height,
            Weight = (int)member.HealthRecord.Weight,
            BloodType = member.HealthRecord.BloodType.GetDisplayName(),
            Note = member.HealthRecord.Note
        };

        return healthDto;
    }

    public async Task<MemberToUpdateDto?> GetForUpdateAsync(int id, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdAsync(id, ct);

        if (member is null) return null;

        var updateDto = new MemberToUpdateDto
        {
            Name = member.Name,
            PhotoUrl = member.Photo,
            Email = member.Email,
            Phone = member.Phone,
            BuildingNumber = member.Address.BuildingNumber,
            Street = member.Address.Street,
            City = member.Address.City
        };

        return updateDto;
    }

    public async Task<bool> UpdateAsync(int id, MemberToUpdateDto updateDto, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdAsync(id, ct);

        if (member is null) return false;
        if (member.Name != updateDto.Name) return false;

        var email = updateDto.Email.Trim().ToLowerInvariant();
        var phone = updateDto.Phone;

        if (await memberRepo.IsEmailTakenAsync(email, id, ct)) return false;
        if (await memberRepo.IsPhoneTakenAsync(phone, id, ct)) return false;

        member.Email = email;
        member.Phone = phone;
        member.Address.BuildingNumber = updateDto.BuildingNumber.Trim();
        member.Address.Street = updateDto.Street.Trim();
        member.Address.City = updateDto.City.Trim();

        var result = await memberRepo.SaveChangesAsync(ct);

        if (result == 0) return false;

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdWithIncludesAsync(id, ct: ct,
            includes: m => m.HealthRecord);

        if (member is null) return false;

        if (await memberRepo.IsHasUpcomingBookingAsync(id, ct))
            return false;

        memberRepo.Remove(member);
        //healthRepo.Remove(member.HealthRecord);

        return (await memberRepo.SaveChangesAsync(ct)) > 0;
    }

    public async Task<MemberDeleteDto?> GetForActivateAsync(int id, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdIncludingDeletedAsync(id, ct);

        if (member is null) return null;

        var activateDto = new MemberDeleteDto
        {
            Id = member.Id,
            Name = member.Name
        };

        return activateDto;
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct = default)
    {
        var member = await memberRepo.GetByIdWithIncludesAsync(id, includeDeleted: true, ct: ct,
            includes: [m => m.HealthRecord, m => m.Bookings, m => m.Memberships]);

        if (member is null) return false;

        member.IsDeleted = false;
        member.DeletedAt = null;

        member.HealthRecord.IsDeleted = false;
        member.HealthRecord.DeletedAt = null;

        if (member.Bookings.Count > 0)
        {
            foreach (var booking in member.Bookings)
            {
                booking.IsDeleted = false;
                booking.DeletedAt = null;
            }
        }

        if (member.Memberships.Count > 0)
        {
            foreach (var membership in member.Memberships)
            {
                membership.IsDeleted = false;
                membership.DeletedAt = null;
            }
        }

        memberRepo.Update(member);

        var result = await memberRepo.SaveChangesAsync(ct);

        if (result == 0) return false;

        return true;
    }
}
