using System.ComponentModel.DataAnnotations;

namespace SomaShare.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        public string Campus { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Profile Image URL")]
        public string ProfileImageUrl { get; set; }
    }
}
