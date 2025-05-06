using Data.Models;
using Data.Entities;
using Data.Repositories;
using Business.Models;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ProjectResult> UpdateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(string id);
}

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IStatusService _statusService;
    private readonly IClientService _clientService; 

    public ProjectService(IProjectRepository projectRepository, IStatusService statusService, IClientService clientService)
    {
        _projectRepository = projectRepository;
        _statusService = statusService;
        _clientService = clientService; 
    }

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };
        }

        var clientResult = await _clientService.AddClientAsync(formData.ClientId);
        if (!clientResult.Succeeded && clientResult.StatusCode != 409)
        {
            return new ProjectResult { Succeeded = false, StatusCode = clientResult.StatusCode, Error = clientResult.Error };
        }

        var client = clientResult.Result?.FirstOrDefault();
        if (client == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "Client not found." };
        }

        var statusResult = await _statusService.GetStatusByIdAsync(1);
        if (statusResult.Result == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "Status not found." };
        }

        var status = statusResult.Result;

        var projectEntity = new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate,
            EndDate = formData.EndDate,
            Budget = formData.Budget,
            ClientId = client.Id,
            UserId = formData.UserId,
            StatusId = status.Id
        };

        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                include => include.User != null,
                include => include.Status,
                include => include.Client
            );

        if (!response.Succeeded || response.Result == null)
        {
            return new ProjectResult<IEnumerable<Project>> { Succeeded = false, StatusCode = 500, Error = "Failed to retrieve projects." };
        }

        // Mappning av Chatgpt
        var projects = response.Result.Select(projectEntity => new Project
        {
            Id = projectEntity.Id,
            ProjectName = projectEntity.ProjectName,
            Description = projectEntity.Description,
            StartDate = projectEntity.StartDate,
            EndDate = projectEntity.EndDate,
            Budget = projectEntity.Budget,
            Client = new Client
            {
                Id = projectEntity.Client.Id,
                ClientName = projectEntity.Client.ClientName
            },
            User = projectEntity.User != null ? new User
            {
                Id = projectEntity.User.Id,
                Email = projectEntity.User.Email
            } : null,
            Status = new Status
            {
                Id = projectEntity.Status.Id,
                StatusName = projectEntity.Status.StatusName
            }
        });

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = projects };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            (
                where: x => x.Id == id,
                include => include.User != null, 
                include => include.Status,
                include => include.Client
            );

        if (!response.Succeeded || response.Result == null)
        {
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
        }

        var projectEntity = response.Result;

        var project = new Project
        {
            Id = projectEntity.Id,
            ProjectName = projectEntity.ProjectName,
            Description = projectEntity.Description,
            StartDate = projectEntity.StartDate,
            EndDate = projectEntity.EndDate,
            Budget = projectEntity.Budget,
            Client = new Client
            {
                Id = projectEntity.Client.Id,
                ClientName = projectEntity.Client.ClientName
            },
            User = projectEntity.User != null ? new User
            {
                Id = projectEntity.User.Id,
                Email = projectEntity.User.Email
            } : null,
            Status = new Status
            {
                Id = projectEntity.Status.Id,
                StatusName = projectEntity.Status.StatusName
            }
        };

        return new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = project };
    }

    public async Task<ProjectResult> UpdateProjectAsync(Project project)
    {
        //Skriven av chatgpt
        var projectEntityResponse = await _projectRepository.GetAsync(x => x.Id == project.Id);
        if (!projectEntityResponse.Succeeded || projectEntityResponse.Result == null)
        {
            return new ProjectResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Project not found."
            };
        }

        var projectEntity = projectEntityResponse.Result;

        projectEntity.ProjectName = project.ProjectName;
        projectEntity.Description = project.Description;
        projectEntity.EndDate = project.EndDate;
        projectEntity.Budget = project.Budget;

        var updateResult = await _projectRepository.UpdateAsync(projectEntity);

        return updateResult.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = updateResult.StatusCode, Error = updateResult.Error };
    }


    public async Task<bool> DeleteProjectAsync(string id)
    {

        if (string.IsNullOrEmpty(id))
            return false;

        var projectEntity = await _projectRepository.GetAsync(
            where: x => x.Id == id
        );

        if (projectEntity?.Result == null)
            return false;

        var deleteResult = await _projectRepository.DeleteAsync(projectEntity.Result);
        return deleteResult.Succeeded;
    }
}
