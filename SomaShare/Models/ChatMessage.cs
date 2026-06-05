using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        [Required]
        public string FromUserId { get; set; } = string.Empty;
        [Required]
        public string ToUserId { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
