using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class ForumService
    {
        private readonly ApplicationDbContext _context;
        public ForumService(ApplicationDbContext context) => _context = context;

        public async Task<List<ForumThread>> GetAllThreadsAsync() =>
            await _context.ForumThreads
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

        public async Task<ForumThread?> GetThreadWithPostsAsync(int id) =>
            await _context.ForumThreads
                .Include(t => t.User)
                .Include(t => t.Posts).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<bool> CreateThreadAsync(ForumThread thread)
        {
            _context.ForumThreads.Add(thread);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPostAsync(ForumPost post)
        {
            _context.ForumPosts.Add(post);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}