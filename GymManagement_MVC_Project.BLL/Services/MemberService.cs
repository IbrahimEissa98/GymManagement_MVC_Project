using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Extensions;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class MemberService(
    IUnitOfWork unitOfWork,
    IDateTimeProvider clock
    ) : IMemberService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<IReadOnlyList<MemberIndexDto>>> GetAllAsync(CancellationToken ct)
    {
        var members = await _unitOfWork.Members.GetAllIncludingDeletedAsync(ct);

        return Result<IReadOnlyList<MemberIndexDto>>.Success([.. members.Select(m => new MemberIndexDto
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Gender = m.Gender.ToString(),
            Phone = m.Phone,
            PhotoUrl = m.Photo,
            IsDeleted = m.IsDeleted
        })]);
    }

    public async Task<Result> CreateAsync(MemberCreateDto createDto, CancellationToken ct = default)
    {
        var email = createDto.Email.Trim().ToLower();
        if (await _unitOfWork.Members.IsEmailTakenAsync(email, ct: ct))
            return Result.Failure("Email is already taken.", ErrorType.Validation, nameof(createDto.Email));

        var phone = createDto.Phone.Trim().ToLower();
        if (await _unitOfWork.Members.IsPhoneTakenAsync(createDto.Phone, ct: ct))
            return Result.Failure("Phone is already taken.", ErrorType.Validation, nameof(createDto.Phone));

        if (!Enum.TryParse(createDto.Gender, true, out GenderTypes gender))
            return Result.Failure("Gender not valid", ErrorType.NotFound, nameof(createDto.Gender));

        if (!Enum.TryParse(createDto.HealthRecord.BloodType, true, out BloodTypes bloodType))
            return Result.Failure("Blood Type not valid", ErrorType.NotFound, nameof(createDto.HealthRecord.BloodType));

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
            JoinDate = _clock.Today
        };

        await _unitOfWork.Members.AddAsync(member, ct);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to create member.", ErrorType.Failure);
    }

    public async Task<Result<MemberDetailsDto>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdWithMembershipAsync(id, _clock.UtcNow, ct);

        if (member is null)
            return Result<MemberDetailsDto>.Failure("Member not found.", ErrorType.NotFound);

        var membership = member.Memberships.FirstOrDefault();

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

        return Result<MemberDetailsDto>.Success(memberDto);
    }

    public async Task<Result<HealthRecordDetailsDto>> GetHealthRecordAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdWithIncludesAsync(id, ct: ct, includes: m => m.HealthRecord);

        if (member is null)
            return Result<HealthRecordDetailsDto>.Failure("Health Record not found.", ErrorType.NotFound);

        var healthDto = new HealthRecordDetailsDto
        {
            PhotoUrl = member.Photo,
            Name = member.Name,
            Height = (int)member.HealthRecord.Height,
            Weight = (int)member.HealthRecord.Weight,
            BloodType = member.HealthRecord.BloodType.GetDisplayName(),
            Note = member.HealthRecord.Note
        };

        return Result<HealthRecordDetailsDto>.Success(healthDto);
    }

    public async Task<Result<MemberToUpdateDto>> GetForUpdateAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id, ct);

        if (member is null)
            return Result<MemberToUpdateDto>.Failure("Member not found.", ErrorType.NotFound);

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

        return Result<MemberToUpdateDto>.Success(updateDto);
    }

    public async Task<Result> UpdateAsync(int id, MemberToUpdateDto updateDto, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id, ct);

        if (member is null)
            return Result.Failure("Member not found.", ErrorType.NotFound);

        if (member.Name != updateDto.Name)
            return Result.Failure("Member name not allowed to update.", ErrorType.Validation);

        var email = updateDto.Email.Trim().ToLowerInvariant();
        var phone = updateDto.Phone;

        if (await _unitOfWork.Members.IsEmailTakenAsync(email, id, ct))
            return Result.Failure("Email is already taken.", ErrorType.Validation, nameof(updateDto.Email));

        if (await _unitOfWork.Members.IsPhoneTakenAsync(phone, id, ct))
            return Result.Failure("Phone is already taken.", ErrorType.Validation, nameof(updateDto.Phone));

        member.Email = email;
        member.Phone = phone;
        member.Address.BuildingNumber = updateDto.BuildingNumber.Trim();
        member.Address.Street = updateDto.Street.Trim();
        member.Address.City = updateDto.City.Trim();

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to update member.", ErrorType.Failure);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdWithIncludesAsync(id, ct: ct,
            includes: m => m.HealthRecord);

        if (member is null)
            return Result.Failure("Member not found.", ErrorType.NotFound);

        if (await _unitOfWork.Members.IsHasUpcomingBookingAsync(id, _clock.UtcNow, ct))
            return Result.Failure("Can not delete member with upcoming Bookings", ErrorType.Failure);

        _unitOfWork.Members.Remove(member);
        //healthRepo.Remove(member.HealthRecord);

        var result = await _unitOfWork.CommitAsync(ct);
        return result > 0 ? Result.Success() : Result.Failure("Failed to delete member.", ErrorType.Failure);
    }

    public async Task<Result<MemberDeleteDto>> GetForActivateAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdIncludingDeletedAsync(id, ct);

        if (member is null)
            return Result<MemberDeleteDto>.Failure("Member not found.", ErrorType.NotFound);

        var activateDto = new MemberDeleteDto
        {
            Id = member.Id,
            Name = member.Name
        };

        return Result<MemberDeleteDto>.Success(activateDto);
    }

    public async Task<Result> ActivateAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdWithIncludesAsync(id, includeDeleted: true, ct: ct,
            includes: [m => m.HealthRecord, m => m.Bookings, m => m.Memberships]);

        if (member is null)
            return Result.Failure("Member not found.", ErrorType.NotFound);

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

        _unitOfWork.Members.Update(member);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to activate member.", ErrorType.Failure);
    }
}
