using System;

namespace CityLibrary.Web.Models
{
    public class BorrowLiteModel
    {
        public int BorrowId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsReturned { get; set; }
        public string UserFullName { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}