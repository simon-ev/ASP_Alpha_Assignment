using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class LogInViewModel
{
    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one number.")]
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; } 
}
