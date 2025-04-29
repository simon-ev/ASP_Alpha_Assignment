using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Models;

public class EditProjectViewModel
{
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Members { get; set; } = [];
    public IEnumerable<SelectListItem> Statuses { get; set; } = [];

}