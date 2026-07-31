using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.BLL.ViewModels.Member;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class MemberService(IMemberRepository memberRepo) : IMemberService
{
    public async Task<IReadOnlyList<MemberIndexViewModel>> GetAllAsync(CancellationToken ct)
    {
        var members = await memberRepo.GetAllAsync(ct);

        return [.. members.Select(m => new MemberIndexViewModel
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Gender = m.Gender.ToString(),
            Phone = m.Phone,
            PhotoUrl = m.Photo
        })];
    }

    public async Task<bool> CreateAsync(MemberCreateViewModel createViewModel, CancellationToken ct = default)
    {
        var email = createViewModel.Email.Trim().ToLower();
        if (await memberRepo.IsEmailExistAsync(email, ct))
            return false;

        var phone = createViewModel.Phone.Trim().ToLower();
        if (await memberRepo.IsPhoneExistAsync(createViewModel.Phone, ct))
            return false;

        if (!Enum.TryParse(createViewModel.Gender, true, out GenderTypes gender))
            return false;

        if (!Enum.TryParse(createViewModel.HealthRecord.BloodType, true, out BloodTypes bloodType))
            return false;

        var member = new Member
        {
            Name = createViewModel.Name,
            Email = email,
            Phone = phone,
            DateOfBirth = createViewModel.DateOfBirth,
            Gender = gender,
            Address = new Address
            {
                City = createViewModel.City,
                Street = createViewModel.Street,
                BuildingNumber = createViewModel.BuildingNumber,
            },
            HealthRecord = new HealthRecord
            {
                Height = createViewModel.HealthRecord.Height,
                Weight = createViewModel.HealthRecord.Weight,
                Note = createViewModel.HealthRecord.Note,
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
