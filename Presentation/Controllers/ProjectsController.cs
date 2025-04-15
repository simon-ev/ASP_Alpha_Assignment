using System.Threading.Tasks;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    public async Task<IActionResult> Index()
    {

        var model = new ProjectsViewModel
        {
            Projects ? await _projectService.GetProjectsAsync(),
        };

        return View(model);
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
        return Json(new {});
    }
}
