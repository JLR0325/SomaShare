using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace SomaShare.Models
{
    public class Offer
    {
        public int Id { get; set; }
        [Range(0, 10000)]
        public decimal Amount { get; set; }
        [Required, StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
        public DateTime OfferDate { get; set; } = DateTime.UtcNow;

        public int TextbookId { get; set; }
        public Textbook Textbook { get; set; } = null!;

        public string UserId { get; set; } = string.Empty; // Buyer who made offer
        public ApplicationUser User { get; set; } = null!;

        // Compatibility properties - keep minimal changes to resolve CS1061 usage across views/controllers
        public string BuyerId
        {
            get => UserId;
            set => UserId = value;
        }

        public DateTime CreatedAt
        {
            get => OfferDate;
            set => OfferDate = value;
        }

        // Seller/UserId of the textbook being offered on. This is kept as a separate stored field for compatibility with existing queries.
        public string TextbookUserId { get; set; } = string.Empty;

        public Transaction? Transaction { get; set; }
    }
}
