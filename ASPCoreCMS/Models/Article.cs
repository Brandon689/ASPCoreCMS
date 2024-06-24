namespace ASPCoreCMS.Models;

public class Article : ContentItem
{
    public string Body { get; set; }
    public List<string> Tags { get; set; }
}