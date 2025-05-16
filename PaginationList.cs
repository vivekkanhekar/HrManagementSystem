namespace HrManagementSystem
{
    public class PaginationList<T> : List<T>
    {
        public int PageIndex { get; set; }

        public int TotalCount { get; set; }
        public PaginationList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalCount = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalCount;

        public static PaginationList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            if (source != null)
            {
                var count = source.Count;
                var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return new PaginationList<T>(items, count, pageIndex, pageSize);
            }
            return null;
        }




    }
}
