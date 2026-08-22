using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Trainer;
using GymManagement_MVC_Project.BLL.Extensions.Mapping;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class TrainerService(IUnitOfWork unitOfWork, IDateTimeProvider clock) : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<IReadOnlyList<TrainerIndexDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var trainers = await _unitOfWork.Trainers.GetAllIncludingDeletedAsync(ct);

        return Result<IReadOnlyList<TrainerIndexDto>>.Success
            ([.. trainers.Select(t => t.GetTrainerIndexDto())]);
    }

    public async Task<Result> CreateAsync(TrainerCreateDto createDto, CancellationToken ct = default)
    {
        var email = createDto.Email.Trim().ToLower();
        if (await _unitOfWork.Trainers.IsEmailTakenAsync(email, ct: ct))
            return Result.Failure("Email is already exists.", ErrorType.Validation, nameof(createDto.Email));

        if (await _unitOfWork.Trainers.IsPhoneTakenAsync(createDto.Phone, ct: ct))
            return Result.Failure("Phone is already exists.", ErrorType.Validation, nameof(createDto.Phone));

        if (!Enum.TryParse(createDto.Gender, true, out GenderTypes gender))
            return Result.Failure("Gender not valid", ErrorType.NotFound, nameof(createDto.Gender));

        if (!Enum.TryParse(createDto.Specialties, true, out TrainerSpecialties specialties))
            return Result.Failure("Specialties not valid", ErrorType.NotFound, nameof(createDto.Specialties));

        var trainer = createDto.GetCreateTrainer(_clock);

        await _unitOfWork.Trainers.AddAsync(trainer, ct);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0
            ? Result.Success()
            : Result.Failure("Failed to create Trainer.", ErrorType.Failure);
    }

    public async Task<Result<TrainerDetailsDto>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null)
            return Result<TrainerDetailsDto>.Failure("Trainer not found.", ErrorType.NotFound);

        var detailsDto = trainer.GetTrainerDetailsDto();

        return Result<TrainerDetailsDto>.Success(detailsDto);
    }

    public async Task<Result<TrainerEditDto>> GetForEditAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null)
            return Result<TrainerEditDto>.Failure("Trainer not found.", ErrorType.NotFound);

        var editDto = trainer.GetTrainerEditDto();

        return Result<TrainerEditDto>.Success(editDto);
    }

    public async Task<Result> EditAsync(int id, TrainerEditDto editDto, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null)
            return Result.Failure("Trainer not found", ErrorType.NotFound);

        if (trainer.Name != editDto.Name)
            return Result.Failure("Can not update trainer name.", ErrorType.Validation);

        var email = editDto.Email.Trim().ToLowerInvariant();
        if (await _unitOfWork.Trainers.IsEmailTakenAsync(email, includeId: id, ct))
            return Result.Failure("Email is already exists.", ErrorType.Validation, nameof(editDto.Email));

        if (await _unitOfWork.Trainers.IsPhoneTakenAsync(editDto.Phone, includeId: id, ct))
            return Result.Failure("Phone is already exists.", ErrorType.Validation, nameof(editDto.Phone));

        if (!Enum.TryParse(editDto.Specialties, true, out TrainerSpecialties specialties))
            return Result.Failure("Specialties not valid", ErrorType.Validation, nameof(editDto.Specialties));

        trainer.SetTrainerUpdates(editDto);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0
            ? Result.Success()
            : Result.Failure("Failed to update trainer.", ErrorType.Failure);
    }

    public async Task<Result<TrainerDeleteDto>> GetForDeleteOrRestoreAsync(int id, bool IsDelete = true, CancellationToken ct = default)
    {
        Trainer? trainer;
        if (IsDelete)
            trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);
        else
            trainer = await _unitOfWork.Trainers.GetByIdIncludingDeletedAsync(id, ct);

        if (trainer is null)
            return Result<TrainerDeleteDto>.Failure("Trainer not found.", ErrorType.NotFound);

        var deleteDto = new TrainerDeleteDto
        {
            Id = id,
            Name = trainer.Name
        };

        return Result<TrainerDeleteDto>.Success(deleteDto);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null)
            return Result.Failure("Trainer not found.", ErrorType.NotFound);

        if (await _unitOfWork.Trainers.IsHasScheduledSessionsAsync(id, _clock.UtcNow, ct))
            return Result.Failure("Can not deactivate Trainer with schedule sessions.", ErrorType.Failure);

        _unitOfWork.Trainers.Remove(trainer);

        return (await _unitOfWork.CommitAsync(ct)) > 0
            ? Result.Success()
            : Result.Failure("Failed to deactivate Trainer.", ErrorType.Failure);
    }

    public async Task<Result> ActivateAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdWithIncludesAsync
            (
                id,
                includeDeleted: true,
                ct: ct,
                includes: [t => t.Sessions]
            );

        if (trainer is null)
            return Result.Failure("Trainer not found.", ErrorType.NotFound);

        trainer.IsDeleted = false;
        trainer.DeletedAt = null;

        foreach (var session in trainer.Sessions)
        {
            session.IsDeleted = false;
            session.DeletedAt = null;
        }

        _unitOfWork.Trainers.Update(trainer);

        return (await _unitOfWork.CommitAsync(ct)) > 0
            ? Result.Success()
            : Result.Failure("Failed to activate trainer.", ErrorType.Failure);
    }
}
