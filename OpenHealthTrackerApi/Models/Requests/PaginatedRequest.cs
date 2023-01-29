namespace OpenHealthTrackerApi.Models;

public class PaginatedRequest
{
     public int ResultsPerPage { get; set; } = 10;
     public int Page { get; set; } = 1;
}