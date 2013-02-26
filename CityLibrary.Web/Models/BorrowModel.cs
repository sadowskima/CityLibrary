using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityLibrary.Web.Models
{
    public class BorrowModel
    {
    
        public partial class Borrow
        {
            public int BorrowId { get; set; }
            public int UserId { get; set; }
            public int BookId { get; set; }
            public System.DateTime FromDate { get; set; }
            public System.DateTime ToDate { get; set; }
            public bool IsReturned { get; set; }   

        }
    }
}