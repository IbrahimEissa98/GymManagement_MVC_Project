using GymManagement_MVC_Project.BLL.DTOs.Trainer;
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

    public async Task<IReadOnlyList<TrainerIndexDto>> GetAllAsync(CancellationToken ct = default)
    {
        var trainers = await _unitOfWork.Trainers.GetAllIncludingDeletedAsync(ct);

        return [..trainers.Select(t => new TrainerIndexDto {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone =t.Phone,
            Specialize = t.Specialties.ToString(),
            IsDeleted = t.IsDeleted
        })];
    }

    public async Task<bool> CreateAsync(TrainerCreateDto createDto, CancellationToken ct = default)
    {
        var email = createDto.Email.Trim().ToLower();
        if (await _unitOfWork.Trainers.IsEmailTakenAsync(email, ct: ct))
            return false;

        if (await _unitOfWork.Trainers.IsPhoneTakenAsync(createDto.Phone, ct: ct))
            return false;

        Enum.TryParse(createDto.Gender, true, out GenderTypes gender);
        Enum.TryParse(createDto.Specialties, true, out TrainerSpecialties specialties);

        var trainer = new Trainer
        {
            Name = createDto.Name,
            Email = email,
            Phone = createDto.Phone,
            DateOfBirth = createDto.DateOfBirth,
            Gender = gender,
            Specialties = specialties,
            Address = new Address
            {
                City = createDto.City,
                Street = createDto.Street,
                BuildingNumber = createDto.BuildingNumber,
            },
            HireDate = _clock.Today
        };

        await _unitOfWork.Trainers.AddAsync(trainer, ct);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0;
    }

    public async Task<TrainerDetailsDto?> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null) return null;

        var address = string.Join(" - ", trainer.Address.BuildingNumber,
                                                trainer.Address.Street,
                                                trainer.Address.City);

        var detailsDto = new TrainerDetailsDto
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            Specialties = trainer.Specialties.ToString(),
            DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
            Gender = trainer.Gender.ToString(),
            Address = address
        };

        return detailsDto;
    }

    public async Task<TrainerEditDto?> GetForEditAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null) return null;

        var editDto = new TrainerEditDto
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            BuildingNumber = trainer.Address.BuildingNumber,
            Street = trainer.Address.Street,
            City = trainer.Address.City,
            Specialties = trainer.Specialties.ToString()
        };

        return editDto;
    }

    public async Task<bool> EditAsync(int id, TrainerEditDto editDto, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null)
            return false;

        if (trainer.Name != editDto.Name)
            return false;

        var email = editDto.Email.Trim().ToLowerInvariant();

        if (await _unitOfWork.Trainers.IsEmailTakenAsync(email, includeId: id, ct))
            return false;

        if (await _unitOfWork.Trainers.IsPhoneTakenAsync(editDto.Phone, includeId: id, ct))
            return false;

        if (!Enum.TryParse(editDto.Specialties, true, out TrainerSpecialties specialties))
            return false;

        trainer.Email = email;
        trainer.Phone = editDto.Phone;
        trainer.Address.BuildingNumber = editDto.BuildingNumber;
        trainer.Address.Street = editDto.Street;
        trainer.Address.City = editDto.City;
        trainer.Specialties = specialties;

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0;
    }

    public async Task<TrainerDeleteDto?> GetForDeleteOrRestoreAsync(int id, bool IsDelete = true, CancellationToken ct = default)
    {
        Trainer? trainer;
        if (IsDelete)
            trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);
        else
            trainer = await _unitOfWork.Trainers.GetByIdIncludingDeletedAsync(id, ct);

        if (trainer is null)
            return null;

        var deleteDto = new TrainerDeleteDto
        {
            Id = id,
            Name = trainer.Name
        };

        return deleteDto;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(id, ct);

        if (trainer is null)
            return false;

        if (await _unitOfWork.Trainers.IsHasScheduledSessionsAsync(id, _clock.UtcNow, ct))
            return false;

        _unitOfWork.Trainers.Remove(trainer);

        return (await _unitOfWork.CommitAsync(ct)) > 0;
    }

    public async Task<bool> ActivateAsync(int id, CancellationToken ct = default)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdWithIncludesAsync
            (
                id,
                includeDeleted: true,
                ct: ct,
                includes: [t => t.Sessions]
            );

        if (trainer is null)
            return false;

        trainer.IsDeleted = false;
        trainer.DeletedAt = null;

        foreach (var session in trainer.Sessions)
        {
            session.IsDeleted = false;
            session.DeletedAt = null;
        }

        _unitOfWork.Trainers.Update(trainer);

        return (await _unitOfWork.CommitAsync(ct)) > 0;
    }
}
