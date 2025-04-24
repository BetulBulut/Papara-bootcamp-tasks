namespace MovieStore.Schema;

public class BaseResponse
{
    public long Id { get; set; }
    public DateTime InsertedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}