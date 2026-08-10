using GymManagement_MVC_Project.BLL.DTOs.Trainer;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;

namespace GymManagement_MVC_Project.BLL.Extensions.Mapping;

public static class TrainerDtoMapping
{
    public static TrainerIndexDto GetTrainerIndexDto(this Trainer trainer)
    {
        return new TrainerIndexDto
        {
            Id = trainer.Id,
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            Specialize = trainer.Specialties.ToString(),
            IsDeleted = trainer.IsDeleted
        };
    }

    public static Trainer GetCreateTrainer(this TrainerCreateDto createDto, IDateTimeProvider date)
    {
        var email = createDto.Email.Trim().ToLower();
        Enum.TryParse(createDto.Gender, true, out GenderTypes gender);
        Enum.TryParse(createDto.Specialties, true, out TrainerSpecialties specialties);

        return new Trainer
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
            HireDate = date.Today
        };
    }

    public static TrainerDetailsDto GetTrainerDetailsDto(this Trainer trainer)
    {
        var address = string.Join(" - ", trainer.Address.BuildingNumber,
                                                trainer.Address.Street,
                                                trainer.Address.City);

        return new TrainerDetailsDto
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            Specialties = trainer.Specialties.ToString(),
            DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
            Gender = trainer.Gender.ToString(),
            Address = address
        };
    }

    public static TrainerEditDto GetTrainerEditDto(this Trainer trainer)
    {
        return new TrainerEditDto
        {
            Name = trainer.Name,
            Email = trainer.Email,
            Phone = trainer.Phone,
            BuildingNumber = trainer.Address.BuildingNumber,
            Street = trainer.Address.Street,
            City = trainer.Address.City,
            Specialties = trainer.Specialties.ToString()
        };
    }

    public static void SetTrainerUpdates(this Trainer trainer, TrainerEditDto editDto)
    {
        var email = editDto.Email.Trim().ToLowerInvariant();
        Enum.TryParse(editDto.Specialties, true, out TrainerSpecialties specialties);

        trainer.Email = email;
        trainer.Phone = editDto.Phone;
        trainer.Address.BuildingNumber = editDto.BuildingNumber;
        trainer.Address.Street = editDto.Street;
        trainer.Address.City = editDto.City;
        trainer.Specialties = specialties;
    }
}
