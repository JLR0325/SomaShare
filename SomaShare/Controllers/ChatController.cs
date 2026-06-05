using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Chat/Index - List all conversations
        public async Task<IActionResult> Index()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                var messages = _context.ChatMessages
                    .Where(m => m.FromUserId == currentUserId || m.ToUserId == currentUserId)
                    .OrderByDescending(m => m.SentAt)
                    .Include(m => m.FromUser)
                    .Include(m => m.ToUser)
                    .ToList();

                return View(messages);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Chat/Conversation?userId=xxx - View conversation with a specific user
        public async Task<IActionResult> Conversation(string userId)
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                if (string.IsNullOrEmpty(userId))
                {
                    // If no userId provided, show list of conversations
                    var messages = _context.ChatMessages
                        .Where(m => m.FromUserId == currentUserId || m.ToUserId == currentUserId)
                        .OrderByDescending(m => m.SentAt)
                        .ToList();
                    return View("Index", messages);
                }

                // Fetch other user details
                var otherUser = await _userManager.FindByIdAsync(userId);
                if (otherUser == null)
                    return NotFound("User not found");

                // Get conversation messages
                var conversation = _context.ChatMessages
                    .Where(m => (m.FromUserId == currentUserId && m.ToUserId == userId) ||
                               (m.FromUserId == userId && m.ToUserId == currentUserId))
                    .OrderBy(m => m.SentAt)
                    .ToList();

                // Set ViewBag properties for the view
                ViewBag.OtherUserId = userId;
                ViewBag.OtherUserName = otherUser.FullName ?? otherUser.UserName;
                ViewBag.OtherUserInstitution = otherUser.Institution ?? "N/A";
                ViewBag.OtherUserCourse = otherUser.Course ?? "N/A";
                ViewBag.OtherUserProfileImage = otherUser.ProfileImageUrl;
                ViewBag.OtherUserRating = 4.5; // Default rating; can be calculated from Reviews table later

                return View(conversation);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Chat/SendMessage - Send a message to another user
        [HttpPost]
        public async Task<IActionResult> SendMessage(string toUserId, string message)
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                if (string.IsNullOrEmpty(toUserId) || string.IsNullOrEmpty(message))
                    return BadRequest("User ID and message are required");

                var chatMessage = new ChatMessage
                {
                    FromUserId = currentUserId,
                    ToUserId = toUserId,
                    Message = message,
                    SentAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();

                return RedirectToAction("Conversation", new { userId = toUserId });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Chat/StartConversation?userId=xxx - Initiate or navigate to a conversation
        public async Task<IActionResult> StartConversation(string userId)
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);

                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");

                return RedirectToAction("Conversation", new { userId = userId });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
}
