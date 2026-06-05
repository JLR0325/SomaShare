using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService) => _chatService = chatService;

        public async Task<IActionResult> Conversation(string withUserId)
        {
            var me = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var conv = await _chatService.GetConversationAsync(me, withUserId);
            ViewBag.WithUserId = withUserId;
            return View(conv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(string toUserId, string message)
        {
            var from = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var msg = new ChatMessage { FromUserId = from, ToUserId = toUserId, Message = message };
            await _chatService.SendMessageAsync(msg);
            return RedirectToAction("Conversation", new { withUserId = toUserId });
        }
    }
}
