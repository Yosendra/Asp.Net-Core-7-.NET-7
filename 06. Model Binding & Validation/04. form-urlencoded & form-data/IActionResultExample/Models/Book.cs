using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Models;

public class Book
{
    public int? BookId { get; set; }

    //[FromRoute]
    public string? Author { get; set; }

    public override string ToString()
    {
        return $"Book object - Book id: {BookId}, Author: {Author}"; 
    }
}
