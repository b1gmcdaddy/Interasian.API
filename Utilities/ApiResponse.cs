using Interasian.API.DTOs;

namespace Interasian.API.Utilities
{
	public class ApiResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; } = null!;
		public object? Data { get; set; }
		public PaginationMetadata? Pagination { get; set; }

		public ApiResponse(bool success, string message, object? data,
		PaginationMetadata? pagination = null)
		{
			Success = success;
			Message = message;	
			Data = data;
			Pagination = pagination;
		}
	}

		// Pagination Details to be placed in API response
		public class PaginationMetadata
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public bool HasPrevious { get; set; }
		public bool HasNext { get; set; }

		public static PaginationMetadata FromPagedList<T>(PagedList<T> pagedList)
		{
			return new PaginationMetadata
			{
				CurrentPage = pagedList.CurrentPage,
				TotalPages = pagedList.TotalPages,
				PageSize = pagedList.PageSize,
				TotalCount = pagedList.TotalCount,
				HasPrevious = pagedList.HasPrevious,
				HasNext = pagedList.HasNext
			};
		}
	}
}
