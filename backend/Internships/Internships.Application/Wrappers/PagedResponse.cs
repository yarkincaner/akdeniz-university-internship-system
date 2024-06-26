using System;

namespace Internships.Core.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public PagedResponse(T data, int pageNumber, int pageSize, int totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Message = null;
            Succeeded = true;
            Errors = null;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}