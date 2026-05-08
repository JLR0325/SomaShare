using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class OfferService
    {
        private readonly ApplicationDbContext _context;
        public OfferService(ApplicationDbContext context) => _context = context;

        public async Task<bool> CreateOfferAsync(Offer offer, string userId)
        {
            // Prevent offering on own book
            var textbook = await _context.Textbooks.FirstOrDefaultAsync(t => t.Id == offer.TextbookId && !t.IsSold);
            if (textbook == null || textbook.UserId == userId) return false;

            offer.UserId = userId;
            offer.Status = "Pending";
            _context.Offers.Add(offer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AcceptOfferAsync(int offerId, string sellerId)
        {
            var offer = await _context.Offers.Include(o => o.Textbook)
                .FirstOrDefaultAsync(o => o.Id == offerId);
            if (offer == null || offer.Textbook.UserId != sellerId || offer.Status != "Pending" || offer.Textbook.IsSold)
                return false;

            // Mark textbook as sold
            offer.Textbook.IsSold = true;

            // Reject all other pending offers for this textbook
            var others = await _context.Offers
                .Where(o => o.TextbookId == offer.TextbookId && o.Id != offerId && o.Status == "Pending")
                .ToListAsync();
            foreach (var o in others) o.Status = "Rejected";

            // Create transaction
            var transaction = new Transaction
            {
                OfferId = offer.Id,
                FinalPrice = offer.Amount,
                BuyerId = offer.UserId,
                SellerId = sellerId,
                Completed = false
            };
            _context.Transactions.Add(transaction);
            offer.Status = "Accepted";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectOfferAsync(int offerId, string sellerId)
        {
            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
            if (offer == null || offer.Textbook.UserId != sellerId || offer.Status != "Pending")
                return false;
            offer.Status = "Rejected";
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Offer>> GetOffersByBuyerAsync(string userId) =>
            await _context.Offers.Include(o => o.Textbook)
                .Where(o => o.UserId == userId)
                .ToListAsync();

        public async Task<List<Offer>> GetOffersForSellerAsync(string sellerId) =>
            await _context.Offers.Include(o => o.Textbook).Include(o => o.User)
                .Where(o => o.Textbook.UserId == sellerId)
                .ToListAsync();
    }
}