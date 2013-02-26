using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CityLibrary.Web.Models;

namespace CityLibrary.Web.ViewModels
{
    public class BookViewModel
    {
        public BookModel BookModel { get; set; }

        public List<DictBookGenreModel> GenreList { get; set; }


    }
}