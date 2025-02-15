namespace GeoAccessControlAPI.Models.ResponseModels;

public class PaginatedResponseModel<T> : ResponseModel<T>
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int? TotalRecords { get; set; }

    public new static PaginatedResponseModel<T> SuccessResponse(T data, string? message = null, int? pageNumber = null, int? pageSize = null, int? totalRecords = null)
    {
        return new PaginatedResponseModel<T>
        {
            Success = true,
            Message = message,
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords
        };
    }

}
