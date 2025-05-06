namespace Presentation.Models;

public class ProjectsViewModel
{
    public IEnumerable<ProjectViewModel> Projects { get; set; } = [];
    public AddProjectViewModel AddProjectFormData { get; set; } = new()
    {
        ProjectName = string.Empty // Rad skriven av chatgpt
    };
    public EditProjectViewModel EditProjectFormData { get; set; } = new();

    public int AllCount { get; set; }
    public int StartedCount { get; set; }
    public int CompletedCount { get; set; }
}
