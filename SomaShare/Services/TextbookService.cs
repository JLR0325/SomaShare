using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class TextbookService
    {
        private readonly ApplicationDbContext _context;
        public TextbookService(ApplicationDbContext context) => _context = context;

        public async Task<List<Textbook>> GetAllAsync(
            string? searchString,
            string? condition,
            string? campus,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortOrder,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Textbooks
                .Include(t => t.User)
                .Where(t => !t.IsSold);

            // Keyword search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string s = searchString.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(s) ||
                    t.Author.ToLower().Contains(s) ||
                    t.ISBN.ToLower().Contains(s));
            }

            // Condition filter
            if (!string.IsNullOrEmpty(condition))
                query = query.Where(t => t.Condition == condition);

            // Campus filter (case-insensitive)
            if (!string.IsNullOrEmpty(campus))
                query = query.Where(t => t.Campus.ToLower().Contains(campus.ToLower()));

            // Price filters
            if (minPrice.HasValue)
                query = query.Where(t => t.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(t => t.Price <= maxPrice.Value);

            // Sorting
            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(t => t.Price),
                "price_desc" => query.OrderByDescending(t => t.Price),
                "date" => query.OrderByDescending(t => t.ListedDate),
                "author" => query.OrderBy(t => t.Author),
                _ => query.OrderBy(t => t.Title) // default
            };

            // Pagination
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Textbook?> GetByIdAsync(int id) =>
            await _context.Textbooks
                .Include(t => t.User)
                .Include(t => t.Offers).ThenInclude(o => o.User)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<bool> CreateAsync(Textbook textbook)
        {
            _context.Textbooks.Add(textbook);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Textbook textbook)
        {
            _context.Textbooks.Update(textbook);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var book = await _context.Textbooks.FindAsync(id);
            if (book == null || book.UserId != userId) return false;
            _context.Textbooks.Remove(book);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
