using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Models;

public class EditProjectViewModel
{
    public string Id { get; set; }
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Statuses { get; set; } = [];
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }


}