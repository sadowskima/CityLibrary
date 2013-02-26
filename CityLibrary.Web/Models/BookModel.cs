﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using CityLibrary.Web.Models;
using CityLibrary.Core.Dao;

namespace CityLibrary.Web.Models
{
    public class BookModel
    {


        public BookModel()
        {
            //AutoMapper.Mapper.CreateMap<DictBookGenreModel, DictBookGenre>();
            //AutoMapper.Mapper.CreateMap<DictBookGenre, DictBookGenreModel>();
             
            //var tmp_DictBookGenres = new List<DictBookGenre>();
            //var dao = new UserService();

            ////tmp_DictBookGenres = dao.GetGenres();

            //DictBookGenres = new List<DictBookGenreModel>();
            //DictBookGenres = AutoMapper.Mapper.Map<List<DictBookGenre>, List<DictBookGenreModel>>(tmp_DictBookGenres); 
        }


        public int BookId { get; set; }

        [Required(ErrorMessage = "Author required")]
        [StringLength(70, ErrorMessage = "Must be under 70")]
        public string Author { get; set; }
        
        public DateTime? ReleaseDate { get; set; }

        [Required(ErrorMessage = "ISBN required")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Book Genre required")]
        public int BookGenreId { get; set; }

        [Required(ErrorMessage = "Book counter required")]
        public int Count { get; set; }

        [Required(ErrorMessage = "Title required")]
        [StringLength(50, ErrorMessage = "Must be under 50")]
        public string Title { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public List<DictBookGenreModel> DictBookGenres { get; set; }
    }
}