using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class SignUpViewModel
{
    [Required]
    [Display(Name = "Full Name", Prompt = "Enter full name")]
    [DataType(DataType.Text)]
    public string FullName { get; set; } = null!;

    [Required]
    [RegularExpression("^$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression("^$", ErrorMessage = "Invalid password")]
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;


    [Range(typeof(bool), "true", "true")]
    public bool TermsAndConditions { get; set; }
}
