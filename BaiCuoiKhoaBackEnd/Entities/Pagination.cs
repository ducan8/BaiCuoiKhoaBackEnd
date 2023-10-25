namespace BaiCuoiKhoaBackEnd.Entities
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage
        {
            get
            {
                if (PageNumber == 0) return 0;
                var res = TotalCount / PageSize;
                if (TotalCount % PageSize != 0)
                {
                    return res + 1;
                }
                return res;
            }
        }

        public Pagination()
        {
            PageSize = -1;
            PageNumber = 1;
        }
    }
}
