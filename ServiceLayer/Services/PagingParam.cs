namespace ProductApiExample.ServiceLayer.Services
{
    /// <summary>
    /// Pagination settings
    /// </summary>
    public struct PagingParam
    {
        /// <summary>
        /// Index of starting record
        /// </summary>
        /// <remarks>Zero based</remarks>
        public uint Offset { get; set; }

        /// <summary>
        /// Record count (page size)
        /// </summary>
        public uint Limit { get; set; }
    }
}
