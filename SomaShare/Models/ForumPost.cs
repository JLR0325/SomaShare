namespace SomaShare.Models
{
    public class ForumPost
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public int ThreadId { get; set; }
        public ForumThread Thread { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }
}