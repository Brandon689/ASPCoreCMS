using ASPCoreCMS.Data;
using ASPCoreCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCoreCMS.Repositories;

public interface IContentRepository
{
    Task<ContentItem> GetByIdAsync(int id);
    Task<IEnumerable<ContentItem>> GetAllAsync();
    Task CreateAsync(ContentItem item);
    Task UpdateAsync(ContentItem item);
    Task DeleteAsync(int id);
}

public class ContentRepository : IContentRepository
{
    private readonly ApplicationDbContext _context;

    public ContentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContentItem> GetByIdAsync(int id)
    {
        return await _context.Articles.FindAsync(id);
    }

    public async Task<IEnumerable<ContentItem>> GetAllAsync()
    {
        return await _context.Articles.ToListAsync();
    }

    public async Task CreateAsync(ContentItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (item is Article article)
        {
            article.CreatedDate = DateTime.UtcNow;
            article.ModifiedDate = DateTime.UtcNow;
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Unsupported content type");
        }
    }

    public async Task UpdateAsync(ContentItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (item is Article article)
        {
            article.ModifiedDate = DateTime.UtcNow;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Unsupported content type");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.Articles.FindAsync(id);
        if (item != null)
        {
            _context.Articles.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ContentItem>> GetByAuthorIdAsync(string authorId)
    {
        return await _context.Articles
            .Where(a => a.AuthorId == authorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ContentItem>> SearchAsync(string searchTerm)
    {
        return await _context.Articles
            .Where(a => a.Title.Contains(searchTerm) || a.Body.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<IEnumerable<ContentItem>> GetRecentAsync(int count)
    {
        return await _context.Articles
            .OrderByDescending(a => a.CreatedDate)
            .Take(count)
            .ToListAsync();
    }
}