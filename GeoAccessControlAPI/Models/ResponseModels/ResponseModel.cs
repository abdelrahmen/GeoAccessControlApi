namespace GeoAccessControlAPI.Models.ResponseModels;

public class ResponseModel<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
   

    public static ResponseModel<T> SuccessResponse(T data, string? message = null)
    {
        return new ResponseModel<T>
        {
            Success = true,
            Message = message,
            Data = data,
        };
    }

    public static ResponseModel<T> Error(string message)
    {
        return new ResponseModel<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}
