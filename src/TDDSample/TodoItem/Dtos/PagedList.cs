namespace TDDSample.TodoItem.Dtos
{
    public class PagedList<T>
        where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public IEnumerable<T>? Results { get; set; }

        public PagedList<TR> To<TR>(Func<T, TR> converter)
            where TR : class
        {
            return new PagedList<TR>
            {
                Results = Results?.Select(converter),
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalNumberOfPages = TotalNumberOfPages,
                TotalNumberOfRecords = TotalNumberOfRecords,
            };
        }
    }
}
