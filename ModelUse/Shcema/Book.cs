namespace ModelUse.Schema;

public class BookRequest 
{
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    
}

public class BookResponse 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }
    public decimal Price { get; set; }
}