using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;
        public TransactionService(ApplicationDbContext context) => _context = context;

        public async Task<List<Transaction>> GetUserTransactionsAsync(string userId) =>
            await _context.Transactions
                .Include(t => t.Offer).ThenInclude(o => o.Textbook)
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Where(t => t.BuyerId == userId || t.SellerId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

        public async Task<Transaction?> GetByIdAsync(int id) =>
            await _context.Transactions
                .Include(t => t.Offer).ThenInclude(o => o.Textbook)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<bool> MarkCompletedAsync(int transactionId, string userId)
        {
            var txn = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            if (txn == null || (txn.BuyerId != userId && txn.SellerId != userId)) return false;
            txn.Completed = true;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}