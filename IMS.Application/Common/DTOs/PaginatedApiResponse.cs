namespace IMS.Application.Common.DTOs;

public class PaginatedApiResponse<T>
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }

    public IReadOnlyList<T> Data { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public List<string>? Errors { get; set; }

    public PaginatedApiResponse(IReadOnlyList<T> data, int pageNumber, int pageSize, int totalCount, string? message = null)
    {
        Succeeded = true;
        Message = message;

        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public PaginatedApiResponse(string error)
    {
        Succeeded = false;
        Errors = new List<string> { error };
        Data = Array.Empty<T>();
    }
}
