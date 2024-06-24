namespace ASPCoreCMS.Models;

public abstract class ContentItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string AuthorId { get; set; }
    public ApplicationUser Author { get; set; }
}