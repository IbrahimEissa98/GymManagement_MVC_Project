using AutoMapper;
using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class MemberService(
    IUnitOfWork unitOfWork,
    IDateTimeProvider clock,
    IMapper mapper
    ) : IMemberService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _clock = clock;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IReadOnlyList<MemberIndexDto>>> GetAllAsync(CancellationToken ct)
    {
        var members = await _unitOfWork.Members.GetAllIncludingDeletedAsync(ct);

        return Result<IReadOnlyList<MemberIndexDto>>.Success
                (_mapper.Map<IReadOnlyList<MemberIndexDto>>(members));
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

        var member = _mapper.Map<Member>(createDto);

        await _unitOfWork.Members.AddAsync(member, ct);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to create member.", ErrorType.Failure);
    }

    public async Task<Result<MemberDetailsDto>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdWithMembershipAsync(id, _clock.UtcNow, ct);

        if (member is null)
            return Result<MemberDetailsDto>.Failure("Member not found.", ErrorType.NotFound);

        var memberDto = _mapper.Map<MemberDetailsDto>(member);

        return Result<MemberDetailsDto>.Success(memberDto);
    }

    public async Task<Result<HealthRecordDetailsDto>> GetHealthRecordAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdWithIncludesAsync(id, ct: ct, includes: m => m.HealthRecord);

        if (member is null)
            return Result<HealthRecordDetailsDto>.Failure("Health Record not found.", ErrorType.NotFound);

        var healthDto = _mapper.Map<HealthRecordDetailsDto>(member);

        return Result<HealthRecordDetailsDto>.Success(healthDto);
    }

    public async Task<Result<MemberToUpdateDto>> GetForUpdateAsync(int id, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id, ct);

        if (member is null)
            return Result<MemberToUpdateDto>.Failure("Member not found.", ErrorType.NotFound);

        var updateDto = _mapper.Map<MemberToUpdateDto>(member);

        return Result<MemberToUpdateDto>.Success(updateDto);
    }

    public async Task<Result> UpdateAsync(int id, MemberToUpdateDto updateDto, CancellationToken ct = default)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id, ct);

        if (member is null)
            return Result.Failure("Member not found.", ErrorType.NotFound);

        if (member.Name != updateDto.Name)
            return Result.Failure("Member name not allowed to update.", ErrorType.Validation);

        if (await _unitOfWork.Members.IsEmailTakenAsync(updateDto.Email, id, ct))
            return Result.Failure("Email is already taken.", ErrorType.Validation, nameof(updateDto.Email));

        if (await _unitOfWork.Members.IsPhoneTakenAsync(updateDto.Phone, id, ct))
            return Result.Failure("Phone is already taken.", ErrorType.Validation, nameof(updateDto.Phone));

        _ = _mapper.Map(updateDto, member);

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

        var activateDto = _mapper.Map<MemberDeleteDto>(member);

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
