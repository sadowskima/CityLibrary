//------------------------------------------------------------------------------
// <auto-generated>
//    Ten kod źródłowy został wygenerowany na podstawie szablonu.
//
//    Ręczne modyfikacje tego pliku mogą spowodować nieoczekiwane zachowanie aplikacji.
//    Ręczne modyfikacje tego pliku zostaną zastąpione w przypadku ponownego wygenerowania kodu.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CityLibrary.Core.Dao
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Borrows = new HashSet<Borrow>();
        }
    
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public System.DateTime AddDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<Borrow> Borrows { get; set; }
    }
}
