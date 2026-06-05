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
                .Include(m => m.FromUser)
                .Include(m => m.ToUser)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<bool> SendMessageAsync(ChatMessage message)
        {
            _context.Set<ChatMessage>().Add(message);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Dictionary<string, ChatMessage>> GetUniqueConversationsAsync(string userId)
        {
            var messages = await _context.ChatMessages
                .Where(m => m.FromUserId == userId || m.ToUserId == userId)
                .Include(m => m.FromUser)
                .Include(m => m.ToUser)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            var uniqueConversations = new Dictionary<string, ChatMessage>();
            foreach (var message in messages)
            {
                var otherUserId = message.FromUserId == userId ? message.ToUserId : message.FromUserId;
                if (!uniqueConversations.ContainsKey(otherUserId))
                {
                    uniqueConversations[otherUserId] = message;
                }
            }

            return uniqueConversations;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
