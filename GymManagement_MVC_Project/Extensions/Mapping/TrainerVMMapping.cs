using GymManagement_MVC_Project.BLL.DTOs.Trainer;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.PL.ViewModels.Trainer;

namespace GymManagement_MVC_Project.PL.Extensions.Mapping;

public static class TrainerVMMapping
{
    public static TrainerIndexViewModel GetTrainerIndexVM(this TrainerIndexDto indexDto)
    {
        return new TrainerIndexViewModel
        {
            Id = indexDto.Id,
            Name = indexDto.Name,
            Email = indexDto.Email,
            Phone = indexDto.Phone,
            Specialize = indexDto.Specialize,
            IsDeleted = indexDto.IsDeleted
        };
    }

    public static TrainerCreateDto GetTrainerCreateDto(this TrainerCreateViewModel createVM)
    {
        return new TrainerCreateDto
        {
            Name = createVM.Name,
            Email = createVM.Email,
            Phone = createVM.Phone,
            DateOfBirth = createVM.DateOfBirth,
            Gender = createVM.Gender,
            Specialties = createVM.Specialties,
            City = createVM.City,
            Street = createVM.Street,
            BuildingNumber = createVM.BuildingNumber
        };
    }

    public static TrainerDetailsViewModel GetTrainerDetailsVM(this TrainerDetailsDto detailsDto)
    {
        return new TrainerDetailsViewModel
        {
            Name = detailsDto.Name,
            Email = detailsDto.Email,
            Phone = detailsDto.Phone,
            Specialties = detailsDto.Specialties.ToString(),
            DateOfBirth = detailsDto.DateOfBirth,
            Gender = detailsDto.Gender,
            Address = detailsDto.Address
        };
    }

    public static TrainerEditViewModel GetTrainerEditVM(this TrainerEditDto editDto)
    {
        Enum.TryParse(editDto.Specialties, true, out TrainerSpecialties specialties);

        return new TrainerEditViewModel
        {
            Name = editDto.Name,
            Email = editDto.Email,
            Phone = editDto.Phone,
            BuildingNumber = editDto.BuildingNumber,
            Street = editDto.Street,
            City = editDto.City,
            Specialties = specialties
        };
    }

    public static TrainerEditDto GetTrainerEditDto(this TrainerEditViewModel editVM)
    {
        return new TrainerEditDto
        {
            Name = editVM.Name,
            Email = editVM.Email,
            Phone = editVM.Phone,
            BuildingNumber = editVM.BuildingNumber,
            Street = editVM.Street,
            City = editVM.City,
            Specialties = editVM.Specialties.ToString()
        };
    }

    public static TrainerDeleteViewModel GetTrainerDeleteVM(this TrainerDeleteDto deleteDto)
    {
        return new TrainerDeleteViewModel
        {
            Id = deleteDto.Id,
            Name = deleteDto.Name
        };
    }
}
