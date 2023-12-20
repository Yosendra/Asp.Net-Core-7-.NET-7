namespace PartialViewExample.Models;

public class ListModel
{
    public string Title { get; set; }
    public List<string> ListItems { get; set; } = new();
}
