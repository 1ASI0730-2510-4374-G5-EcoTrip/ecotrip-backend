using System.Collections.Generic;
using Experience.Application.DTOs;

namespace Experience.Application.Queries.ListExperience
{
    /// <summary>
    /// Query to list experiences with optional filtering and pagination
    /// </summary>
    public class ListExperiencesQuery
    {
        /// <summary>
        /// Number of items to skip for pagination
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Maximum number of items to return
        /// </summary>
        public int Take { get; set; } = 20;

        /// <summary>
        /// Optional filter by location
        /// </summary>
        public string? LocationFilter { get; set; }

        /// <summary>
        /// Optional filter by minimum price
        /// </summary>
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Optional filter by maximum price
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Optional filter by minimum date
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Optional filter by maximum date
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Property to sort by (e.g., "Date", "Price")
        /// </summary>
        public string SortBy { get; set; } = "Date";

        /// <summary>
        /// Sort direction (true for ascending, false for descending)
        /// </summary>
        public bool SortAscending { get; set; } = true;
    }
}