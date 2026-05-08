using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class WantedAdService
    {
        private readonly ApplicationDbContext _context;
        public WantedAdService(ApplicationDbContext context) => _context = context;

        public async Task<List<WantedAd>> GetAllAsync() =>
            await _context.WantedAds.Include(w => w.User).ToListAsync();

        public async Task<WantedAd?> GetByIdAsync(int id) =>
            await _context.WantedAds.Include(w => w.User).FirstOrDefaultAsync(w => w.Id == id);

        public async Task<bool> CreateAsync(WantedAd ad)
        {
            _context.WantedAds.Add(ad);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(WantedAd ad)
        {
            _context.WantedAds.Update(ad);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var ad = await _context.WantedAds.FindAsync(id);
            if (ad == null || ad.UserId != userId) return false;
            _context.WantedAds.Remove(ad);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
