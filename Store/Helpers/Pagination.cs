namespace Store.Helpers
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int CountOfSpec { get; set; }
        public int CountOfAllItem { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
