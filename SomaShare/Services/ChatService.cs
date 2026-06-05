using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _context;
        public ChatService(ApplicationDbContext context) => _context = context;

        public async Task<List<ChatMessage>> GetConversationAsync(string userAId, string userBId)
        {
            return await _context.Set<ChatMessage>()
                .Where(m => (m.FromUserId == userAId && m.ToUserId == userBId) || (m.FromUserId == userBId && m.ToUserId == userAId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<bool> SendMessageAsync(ChatMessage message)
        {
            _context.Set<ChatMessage>().Add(message);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
