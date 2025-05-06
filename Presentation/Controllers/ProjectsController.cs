using Data.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, IStatusService statusService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IStatusService _statusService = statusService;

    public async Task<IActionResult> Index()
    {
        var serviceProjects = await _projectService.GetProjectsAsync();
        if (!serviceProjects.Succeeded || serviceProjects.Result == null)
        {
            return View("Error");
        }

        var projects = serviceProjects.Result;
        var startedProjects = projects.Where(p => p.Status?.StatusName == "In Progress").ToList();
        var completedProjects = projects.Where(p => p.Status?.StatusName == "Completed").ToList();

        var viewModel = new ProjectsViewModel
        {
            Projects = projects.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget,
                Status = p.Status.StatusName,
                ClientName = p.Client.ClientName
            }),
            StartedCount = startedProjects.Count,
            CompletedCount = completedProjects.Count,
            AllCount = projects.Count()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "User is not authenticated." });
        }

        if (model.Clients == null || !model.Clients.Any())
        {
            return Json(new { success = false, message = "Client is required." });
        }

        var addProjectFormData = new AddProjectFormData
        {
            ProjectName = model.ProjectName,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            ClientId = model.Clients.First().Value,
            UserId = userId,
            StatusId = 1
        };

        var result = await _projectService.CreateProjectAsync(addProjectFormData);

        return Json(new
        {
            success = result.Succeeded,
            statusCode = result.StatusCode,
            error = result.Error
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = await _projectService.GetProjectAsync(model.Id);
        if (!project.Succeeded || project.Result == null)
        {
            return Json(new { success = false, message = "Project not found" });
        }

        //Skriven av chatgpt
        project.Result.ProjectName = model.ProjectName;
        project.Result.Description = model.Description;
        project.Result.EndDate = model.EndDate;
        project.Result.Budget = model.Budget;


        var updateResult = await _projectService.UpdateProjectAsync(project.Result);

        return Json(new
        {
            success = updateResult.Succeeded,
            message = updateResult.Error
        });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Json(new { success = false, message = "Invalid project ID." });
        }

        var success = await _projectService.DeleteProjectAsync(id);
        if (success)
        {
            return Json(new { success = true, id });
        }

        return Json(new { success = false, message = "Failed to delete project." });
    }

    public async Task<IEnumerable<SelectListItem>> SetStatuses(string? currentStatus = null)
    {
        var statusResult = await _statusService.GetStatusesAsync();
        if (!statusResult.Succeeded)
        {
            return new List<SelectListItem>();
        }

        return statusResult.Result.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.StatusName,
            Selected = s.StatusName == currentStatus
        });
    }
}





//public class ProjectsController(IProjectService projectService) : Controller
//{
//    private readonly IProjectService _projectService = projectService;

//    public async Task<IActionResult> Index()
//    {

//        var serviceProjects = await _projectService.GetProjectsAsync();
//        if (!serviceProjects.Succeeded)
//        {
//            return View("Error");
//        }

//        var viewModel = new ProjectsViewModel
//        {
//            Projects = serviceProjects.Result.Select(p => new ProjectViewModel
//            {
//                Id = p.Id,
//                ProjectName = p.ProjectName,
//                Description = p.Description,
//                StartDate = p.StartDate,
//                EndDate = p.EndDate,
//                Budget = p.Budget,
//                Status = p.Status.StatusName
//            }),
//            EditProjectFormData = new EditProjectViewModel
//            {
//                Statuses = SetStatuses()
//            }
//        };

//        return View(viewModel);
//    }


//    public IEnumerable<ProjectViewModel> SetProjects()
//    {
//        var projects = new List<ProjectViewModel>
//        {
//            new() {
//            Id = Guid.NewGuid().ToString(),
//            ProjectName = "Project 1",
//            Description = "Description 1",
//            StartDate = DateTime.Now,
//            EndDate = null,
//            Budget = 1000,
//            Status = "In Progress"
//            }
//        };

//        return projects;
//    }


//    [HttpPost]
//    public async Task<IActionResult> Add(AddProjectViewModel model)
//    {
//        var addProjectFormData = new AddProjectFormData
//        {
//            ProjectName = model.ProjectName,
//            Description = model.Description,
//            StartDate = model.StartDate,
//            EndDate = model.EndDate,
//            Budget = model.Budget
//        };
//        var result = await _projectService.CreateProjectAsync(addProjectFormData);

//        return Json(new { });
//    }

//    [HttpPost]
//    public IActionResult Update(EditProjectViewModel model)
//    {
//        return Json(new { });
//    }

//    [HttpPost]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var success = await _projectService.DeleteProjectAsync(id);
//        if (success)
//        {
//            return Json(new { success = true, id });
//        }

//        return Json(new { success = false, message = "Failed to delete project." });
//    }

//    public IEnumerable<SelectListItem> SetStatuses()
//    {
//        var members = new List<SelectListItem>
//        {
//            new()
//            {
//                Value = "1",
//                Text = "In Progress", Selected = true
//            },
//            new()
//            {
//                Value = "2",
//                Text = "Completed"
//            }
//        };
//        return statuses;
//    }
//}
