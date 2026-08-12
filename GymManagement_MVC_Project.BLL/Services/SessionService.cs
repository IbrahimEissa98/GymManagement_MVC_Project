using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Session;
using GymManagement_MVC_Project.BLL.Extensions.Mapping;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Queries.DTOs;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

public class SessionService(
    IUnitOfWork unitOfWork,
    ISessionQueryService queryService,
    IDateTimeProvider clock) : ISessionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISessionQueryService _queryService = queryService;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<IReadOnlyList<SessionIndexDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var sessions = await _queryService.GetAllSessionsIndexQSAsync(ct);

        var indexDto = sessions.Select(s => s.GetSessionIndexDto());

        return Result<IReadOnlyList<SessionIndexDto>>.Success([.. indexDto]);
    }

    public async Task<Result<SessionIndexDto>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var session = await _queryService.GetSessionDetailsQSAsync(id, ct);

        if (session is null)
            return Result<SessionIndexDto>.Failure("Session not found", ErrorType.NotFound);

        return Result<SessionIndexDto>.Success(session.GetSessionIndexDto());
    }

    public async Task<Result<IReadOnlyList<CategoryLookupItem>>> GetAllCategoryLookupItems(CancellationToken ct = default)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(ct);

        if (categories.Count == 0)
            return Result<IReadOnlyList<CategoryLookupItem>>.Failure("No Categories was found.", ErrorType.NotFound);

        return Result<IReadOnlyList<CategoryLookupItem>>.Success
            ([.. categories.Select(s =>
            new CategoryLookupItem { Id = s.Id, Name = s.Name })]);
    }

    public async Task<Result<IReadOnlyList<TrainerLookupItem>>> GetAllTrainersByCategoryId(int categoryId, CancellationToken ct = default)
    {
        var trainers = await _queryService.GetTrainersByCategoryIdAsync(categoryId, ct) ?? [];

        return Result<IReadOnlyList<TrainerLookupItem>>.Success(trainers);
    }

    public async Task<Result> CreateAsync(SessionCreateDto createDto, CancellationToken ct = default)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(createDto.CategoryId, ct);
        if (category is null)
            return Result.Failure("Selected category is invalid", ErrorType.Validation, nameof(createDto.CategoryId));

        var trainer = await _unitOfWork.Trainers.GetByIdAsync(createDto.TrainerId, ct);
        if (trainer is null)
            return Result.Failure("Selected Trainer is invalid", ErrorType.Validation, nameof(createDto.TrainerId));

        if (createDto.EndDate < createDto.StartDate)
            return Result.Failure("End date must be after start date", ErrorType.Validation, nameof(createDto.EndDate));
        if (createDto.StartDate < _clock.UtcNow)
            return Result.Failure("Start date must be in the future.", ErrorType.Validation, nameof(createDto.StartDate));
        if (createDto.Capacity > 25 || createDto.Capacity < 0)
            return Result.Failure("Capacity must be between 1 and 25", ErrorType.Validation, nameof(createDto.Capacity));

        if (trainer.Specialties != category.Specialties)
            return Result.Failure("Miss match between category and trainer", ErrorType.Validation, nameof(createDto.TrainerId));

        if (!(await _unitOfWork.Sessions.IsTrainerFree(createDto.TrainerId, createDto.StartDate, createDto.EndDate, ct: ct)))
            return Result.Failure("This trainer is not free", ErrorType.Validation, nameof(createDto.TrainerId));

        await _unitOfWork.Sessions.AddAsync(createDto.GetSession(), ct);

        var result = await _unitOfWork.CommitAsync(ct);

        return result > 0 ? Result.Success() : Result.Failure("Failed to create session.", ErrorType.Failure);
    }

    public async Task<Result<SessionEditDto>> GetForEditAsync(int sessionId, CancellationToken ct = default)
    {
        var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId, ct);
        if (session is null)
            return Result<SessionEditDto>.Failure("Session not found.", ErrorType.NotFound);
        if (session.EndDate < _clock.UtcNow)
            return Result<SessionEditDto>.Failure("Can not edit completed session.", ErrorType.Failure);
        if (session.StartDate <= _clock.UtcNow && session.EndDate >= _clock.UtcNow)
            return Result<SessionEditDto>.Failure("Can not edit ongoing session.", ErrorType.Failure);

        return Result<SessionEditDto>.Success(session.GetSessionEditDto());
    }

    public async Task<Result> EditAsync(int sessionId, SessionEditDto editDto, CancellationToken ct = default)
    {
        var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId, ct);
        if (session is null)
            return Result<SessionEditDto>.Failure("Session not found.", ErrorType.NotFound);
        if (session.EndDate < _clock.UtcNow)
            return Result<SessionEditDto>.Failure("Can not edit completed session.", ErrorType.Failure);
        if (session.StartDate <= _clock.UtcNow && session.EndDate >= _clock.UtcNow)
            return Result<SessionEditDto>.Failure("Can not edit ongoing session.", ErrorType.Failure);

        if (session.CategoryId != editDto.CategoryId)
            return Result.Failure("Can not update session category", ErrorType.Conflict);

        var category = await _unitOfWork.Categories.GetByIdAsync(editDto.CategoryId, ct);
        if (category is null)
            return Result.Failure("Can not update session category", ErrorType.Validation);

        var trainer = await _unitOfWork.Trainers.GetByIdAsync(editDto.TrainerId, ct);
        if (trainer is null)
            return Result.Failure("Selected Trainer is invalid", ErrorType.Validation, nameof(editDto.TrainerId));

        if (editDto.EndDate < editDto.StartDate)
            return Result.Failure("End date must be after start date", ErrorType.Validation, nameof(editDto.EndDate));
        if (editDto.StartDate < _clock.UtcNow)
            return Result.Failure("Start date must be in the future.", ErrorType.Validation, nameof(editDto.StartDate));

        if (trainer.Specialties != category.Specialties)
            return Result.Failure("Miss match between category and trainer", ErrorType.Validation, nameof(editDto.TrainerId));

        if (!(await _unitOfWork.Sessions.IsTrainerFree(editDto.TrainerId, editDto.StartDate, editDto.EndDate, excludeSessionId: sessionId, ct: ct)))
            return Result.Failure("This trainer is not free", ErrorType.Validation, nameof(editDto.TrainerId));

        session.TrainerId = editDto.TrainerId;
        session.Description = editDto.Description;
        session.StartDate = editDto.StartDate;
        session.EndDate = editDto.EndDate;

        var result = await _unitOfWork.CommitAsync(ct);
        return result > 0 ? Result.Success() : Result.Failure("Failed to Update session.", ErrorType.Failure);
    }

    public async Task<Result<SessionDeleteDto>> GetForDeleteOrActivateAsync(int sessionId, bool isDelete = true, CancellationToken ct = default)
    {
        var session = await _queryService.GetSessionForDeleteOrActivateQSAsync(sessionId, isDelete, ct);
        if (session is null)
            return Result<SessionDeleteDto>.Failure("Session not found.", ErrorType.NotFound);

        return Result<SessionDeleteDto>.Success(new SessionDeleteDto
        {
            CategoryName = session.CategoryName,
            TrainerName = session.TrainerName
        });
    }

    public async Task<Result> DeleteAsync(int sessionId, CancellationToken ct = default)
    {
        var session = await _unitOfWork.Sessions.GetByIdWithIncludesAsync(sessionId, ct: ct, includes: [sessionId => sessionId.Bookings]);
        if (session is null)
            return Result.Failure("Session not found.", ErrorType.NotFound);

        try
        {
            _unitOfWork.Sessions.Remove(session);
            var result = await _unitOfWork.CommitAsync(ct);
            return result > 0 ? Result.Success() : Result.Failure("Failed to deactivate this session.", ErrorType.Failure);
        }
        catch (Exception)
        {
            return Result.Failure("Failed to deactivate this session.", ErrorType.Failure);
        }
    }

    public async Task<Result> ActivateAsync(int sessionId, CancellationToken ct = default)
    {
        var session = await _unitOfWork.Sessions.GetByIdWithIncludesAsync(
            sessionId,
            includeDeleted: true,
            includes: [sessionId => sessionId.Bookings],
            ct: ct);
        if (session is null)
            return Result.Failure("Session not found.", ErrorType.NotFound);

        try
        {
            session.IsDeleted = false;
            session.DeletedAt = null;

            foreach (var booking in session.Bookings)
            {
                booking.IsDeleted = false;
                booking.DeletedAt = null;
            }

            return (await _unitOfWork.CommitAsync(ct)) > 0
                ? Result.Success()
                : Result.Failure("Failed to activate this session.", ErrorType.Failure);
        }
        catch (Exception)
        {
            return Result.Failure("Failed to activate this session.", ErrorType.Failure);
        }
    }
}
