using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Range(0, 10000)]
        public decimal FinalPrice { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public bool Completed { get; set; } = false;

        public int OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        public string BuyerId { get; set; } = string.Empty;
        public ApplicationUser Buyer { get; set; } = null!;

        public string SellerId { get; set; } = string.Empty;
        public ApplicationUser Seller { get; set; } = null!;
    }
}