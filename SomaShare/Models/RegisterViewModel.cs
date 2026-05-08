using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, StringLength(100)]
        public string FullName { get; set; } = "";

        public string? Institution { get; set; }

        public string? Course { get; set; }

        public string Campus { get; set; } = "";

        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; } = "";

        [DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}
