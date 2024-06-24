using ASPCoreCMS.Models;
using ASPCoreCMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreCMS.Controllers;

[Authorize] // This ensures only authenticated users can access these actions
public class ArticlesController : Controller
{
    private readonly IContentRepository _repository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ArticlesController(IContentRepository repository, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    [AllowAnonymous] // This action can be accessed by anyone
    public async Task<IActionResult> Index()
    {
        var articles = await _repository.GetAllAsync();
        return View(articles);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Article article)
    {
        if (ModelState.IsValid)
        {
            article.AuthorId = _userManager.GetUserId(User);
            article.CreatedDate = DateTime.UtcNow;
            article.ModifiedDate = DateTime.UtcNow;
            await _repository.CreateAsync(article);
            return RedirectToAction(nameof(Index));
        }
        return View(article);
    }

    // Add Edit, Delete, and Details actions
}
