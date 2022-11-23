using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Library.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(60, MinimumLength = 10)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
    /*• Has a UserName – a string with min length 5 and max length 20 (required)
    • Has an Email – a string with min length 10 and max length 60 (required)
    • Has a Password – a string with min length 5 and max length 20 (before hashed)
    – no max length required for a hashed password in the database (required)*/