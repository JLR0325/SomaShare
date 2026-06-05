using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomaShare.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public string FromUserId { get; set; } = string.Empty;
        public ApplicationUser? FromUser { get; set; }

        [Required]
        public string ToUserId { get; set; } = string.Empty;
        public ApplicationUser? ToUser { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
