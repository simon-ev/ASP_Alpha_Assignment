using System.Threading.Tasks;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Presentation.Models;

namespace Presentation.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    public async Task<IActionResult> Index()
    {

        var projects = await _projectService.GetProjectsAsync();
        var viewModel = new ProjectsViewModel
        {
            Projects = SetProjects(),
            EditProjectFormData = new EditProjectViewModel
            {
                Statuses = SetStatuses()
            }
        };

        return View(viewModel);
    }


    public IEnumerable<ProjectViewModel> SetProjects()
    {
        var projects = new List<ProjectViewModel>
        {
            new() {
            Id = Guid.NewGuid().ToString(),
            ProjectName = "Project 1",
            Description = "Description 1",
            StartDate = DateTime.Now,
            EndDate = null,
            Budget = 1000,
            Status = "In Progress"
            }
        };

        return projects;
    }


    [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel model)
        {
            var addProjectFormData = new AddProjectFormData
            {
                ProjectName = model.ProjectName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Budget = model.Budget
            };
            var result = await _projectService.CreateProjectAsync(addProjectFormData);

            return Json(new { });
        }

        [HttpPost]
        public IActionResult Update(EditProjectViewModel model)
        {
            return Json(new { });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(new { });
        }

    private IEnumerable<SelectListItem> SetStatuses()
    {
        var members = new List<SelectListItem>
        {
            new()
            {
                Value = "1",
                Text = "In Progress", Selected = true
            },
            new()
            {
                Value = "2",
                Text = "Completed"
            }
        };
    }
} 

