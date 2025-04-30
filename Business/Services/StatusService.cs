using Data.Repositories;
using Data.Models;
using Business.Models;



namespace Business.Services;

public interface IStatusService
{
    Task<StatusResult<Status>> GetStatusByIdAsync(int id);
    Task<StatusResult<Status>> GetStatusByNameAsync(string statusName);
    Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.Succeeded
            ? new StatusResult<IEnumerable<Status>> { Succeeded = true, StatusCode = 200, Result = result.Result }
            : new StatusResult<IEnumerable<Status>> { Succeeded = false, StatusCode = result.StatusCode, Error = "No statuses found." };
    }

    public async Task<StatusResult<Status>> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        return result.Succeeded
            ? new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = result.Result }
            : new StatusResult<Status> { Succeeded = false, StatusCode = result.StatusCode, Error = "No statuses found." };
    }

    public async Task<StatusResult<Status>> GetStatusByIdAsync(int id)
    {
        var result = await _statusRepository.GetAsync(x => x.Id == id);
        return result.Succeeded
             ? new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = result.Result }
             : new StatusResult<Status> { Succeeded = false, StatusCode = result.StatusCode, Error = "No statuses found." };

    }
}
