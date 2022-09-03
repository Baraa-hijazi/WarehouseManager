namespace WarehouseManager.Core.DTOs;

public class PagedResultDto<T>
{
    public PagedResultDto()
    {
        Result = new List<T>();
    }

    public int? TotalCount { set; get; }
    public IList<T> Result { set; get; }
}