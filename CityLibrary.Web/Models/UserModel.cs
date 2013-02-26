using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CityLibrary.Core.Dao;
using System.ComponentModel.DataAnnotations;

namespace CityLibrary.Web.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name required")]
        [StringLength(35, ErrorMessage = "Must be under 35")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name required")]
        [StringLength(50, ErrorMessage = "Must be under 50")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birthdate required")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Email required!")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }

        public bool HasBooks { get; set; }

        //public ICollection<Borrow> Borrows { get; set; }
    }
}