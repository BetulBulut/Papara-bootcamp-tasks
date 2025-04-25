namespace Base.Models;

public class BaseModel
{
    public int Id { get; set; }
    public DateTime InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}