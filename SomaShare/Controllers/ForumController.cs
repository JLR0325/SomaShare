using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class ForumController : Controller
    {
        private readonly ForumService _forumService;
        public ForumController(ForumService forumService) => _forumService = forumService;

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var threads = await _forumService.GetAllThreadsAsync();
            return View(threads);
        }

        public IActionResult CreateThread() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateThread(ForumThread thread)
        {
            if (!ModelState.IsValid) return View(thread);
            thread.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _forumService.CreateThreadAsync(thread);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var thread = await _forumService.GetThreadWithPostsAsync(id);
            if (thread == null) return NotFound();
            return View(thread);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(int threadId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return RedirectToAction(nameof(Details), new { id = threadId });

            var post = new ForumPost
            {
                ThreadId = threadId,
                Content = content,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
            };
            await _forumService.AddPostAsync(post);
            return RedirectToAction(nameof(Details), new { id = threadId });
        }
    }
}