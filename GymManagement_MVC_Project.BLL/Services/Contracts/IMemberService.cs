using GymManagement_MVC_Project.BLL.ViewModels.Member;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IMemberService
{
    Task<IReadOnlyList<MemberIndexViewModel>> GetAllAsync(CancellationToken ct = default);
    Task<bool> CreateAsync(MemberCreateViewModel createViewModel, CancellationToken ct = default);
}
