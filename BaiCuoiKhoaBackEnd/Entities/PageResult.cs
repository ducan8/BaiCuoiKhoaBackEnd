namespace BaiCuoiKhoaBackEnd.Entities
{
    public class PageResult<T>
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Data { get; set; }
        public PageResult() { }
        public PageResult(Pagination pagination, IEnumerable<T> data)
        {
            Pagination = pagination;
            Data = data;
        }

        public static IEnumerable<T> ToPageResult (Pagination pagination, IEnumerable<T> Data)
        {
            pagination.PageNumber = pagination.PageNumber < 0 ? 1 : pagination.PageNumber;
            var data = Data.Skip(pagination.PageSize * (pagination.PageNumber - 1)).Take(pagination.PageSize);
            return data;
        } 
    }
}
