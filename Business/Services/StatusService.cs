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


        if (!result.Succeeded || result.Result == null)
        {
            return new StatusResult<IEnumerable<Status>>
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = "No statuses found."
            };
        }

        // Mappning av chatgpt
        var statuses = result.Result.Select(entity => new Status
        {
            Id = entity.Id,
            StatusName = entity.StatusName
        });

        return new StatusResult<IEnumerable<Status>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = statuses
        };
    }

    public async Task<StatusResult<Status>> GetStatusByNameAsync(string statusName)
    {

        //Omformaterad av chatgpt
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        if (!result.Succeeded || result.Result == null)
        {
            return new StatusResult<Status>
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = "No statuses found."
            };
        }

        var status = new Status
        {
            Id = result.Result.Id,
            StatusName = result.Result.StatusName
        };

        return new StatusResult<Status>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = status
        };
    }

    public async Task<StatusResult<Status>> GetStatusByIdAsync(int id)
    {
        var result = await _statusRepository.GetAsync(x => x.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new StatusResult<Status>
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = result.Error ?? "No status found."
            };
        }

        var status = new Status
        {
            Id = result.Result.Id,
            StatusName = result.Result.StatusName
        };

        return new StatusResult<Status>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = status
        };
    }
}
