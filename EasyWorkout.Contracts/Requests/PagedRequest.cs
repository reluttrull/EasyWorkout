using System.ComponentModel.DataAnnotations;

namespace EasyWorkout.Contracts.Requests
{
    public class PagedRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int Page { get; init; } = 1;
        [Range(1, 25, ErrorMessage = "Page size must be between 1 and 25.")]
        public int PageSize { get; init; } = 10;
    }
}
