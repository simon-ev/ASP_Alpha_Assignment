namespace Presentation.Models;

public class ProjectsViewModel
{
    public IEnumerable<ProjectViewModel> Projects { get; set; } = [];
    public AddProjectViewModel AddProjectFormData { get; set; } = new();
    public EditProjectViewModel EditProjectFormData { get; set; } = new();

    //Skriven av chatgpt för att kunna räkna antal projekt
    public int AllCount { get; set; }
    public int StartedCount { get; set; }
    public int CompletedCount { get; set; }
}
