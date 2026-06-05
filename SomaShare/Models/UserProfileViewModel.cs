namespace SomaShare.Models;

public class UserProfileViewModel
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Institution { get; set; }
    public string Course { get; set; }
    public string Campus { get; set; }
    public double Rating { get; set; }
    public string ProfileImageUrl { get; set; }
    public int TextbooksCount { get; set; }
    public int ReviewsCount { get; set; }
}
