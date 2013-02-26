using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLibrary.Core.Dao
{
    public partial class User
    {
        public bool HasBooks 
        {
            get 
            {
                return Borrows != null && Borrows.Count > 0; 
            }
        } 
    }
}
