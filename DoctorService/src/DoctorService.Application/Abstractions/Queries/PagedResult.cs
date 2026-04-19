namespace ProductService.Application.Abstractions.Queries
{
    public sealed record Cursor(string? LastStart, string? LastId);

    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];
        public Cursor? NextCursor { get; set; }
        //public int Total { get; set; }
        //public int Page { get; set; }
        //public int PageSize { get; set; }
    }

}
