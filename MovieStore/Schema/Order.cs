using MovieStore.Models;

namespace MovieStore.Schema;

public class OrderRequest : BaseRequest
{
    public int CustomerId { get; set; }
    public int MovieId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PriceAtPurchase { get; set; }
   
}

public class OrderResponse : BaseResponse
{
   
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public CustomerResponse Customer { get; set; }
    public int MovieId { get; set; }
    public MovieResponse Movie { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PriceAtPurchase { get; set;}
}
