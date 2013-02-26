using System.Collections.Generic;
using System.Web.Mvc;
using CityLibrary.Core.Dao;
using CityLibrary.Core.Util;
using CityLibrary.Web.Models;
using CityLibrary.Web.ViewModels;
using CityLibrary.Web.Views.Shared;

namespace CityLibrary.Web.Controllers
{
    public class BookController : Controller
    {
        static BookController()
        {
            AutoMapper.Mapper.CreateMap<Book, BookModel>();
            AutoMapper.Mapper.CreateMap<BookModel, Book>();
            AutoMapper.Mapper.CreateMap<DictBookGenreModel, DictBookGenre>();
            AutoMapper.Mapper.CreateMap<DictBookGenre, DictBookGenreModel>();
            AutoMapper.Mapper.CreateMap<Book, BookViewModel>();
        }

        //
        // GET: /Book/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Book/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Book/Create

        public ActionResult Create()
        {
            return PartialView("_EditBook");
        }

        //
        // POST: /Book/Create

        // Metoda mogłaby być też [HttpGet] ale ja użyje POST (bo jest zdefinowana na stałe)
        // $.ajax <- tu zmieniam 
        [HttpPost]
        public ActionResult CreateView()
        {
            var model = new BookModel();

            //model.BookGenreId = 1;


            return PartialView("_CreateBook", model);
        }

        [HttpPost]
        public ActionResult Create(BookModel book)
        {
            try
            {

                var tmp_book = new Book();
                string err = null;


                tmp_book = AutoMapper.Mapper.Map<Book>(book);
                tmp_book.AddDate = System.DateTime.Now;

                var dao = new UserService();

                err = dao.CreateBook(tmp_book);

                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Book/Edit/5

        [HttpPost]
        public ActionResult EditView(int id)
        {
            var dao = new UserService();
            var bvm = new BookViewModel();
            var book = dao.GetBook(id);

            var model = AutoMapper.Mapper.Map<Book, BookModel>(book);
            bvm.BookModel = model;

            return PartialView("_EditBook", bvm);
        }

        [HttpPost]
        public JsonResult GetGenreList()
        {
            var dao = new UserService();

            List<DictBookGenre> list = dao.GetGenreList();

            var listModel = AutoMapper.Mapper.Map<List<DictBookGenre>, List<DictBookGenreModel>>(list);

            return this.Json(new { Items = listModel });
        }

        [HttpPost]
        public JsonResult GetBooks(SearchCriteria[] searchCriteria, SortOrder[] sort, int page, int pageSize)
        {

            var dao = new UserService();

            int count;
            var list = dao.GetBooks(searchCriteria, sort, page, pageSize, out count);



            var books = AutoMapper.Mapper.Map<List<Book>, List<BookModel>>(list);
            //var books = AutoMapper.Mapper.Map<List<BookModel>>(list);


            return Json(new { Books = books, Count = count });
        }
        //
        // POST: /Book/Edit/5

        [HttpPost]
       // public ActionResult Edit(int id, FormCollection collection)
        public ActionResult Edit(BookModel book)
        {
            try
            {

                var tmp_book = new Book();
                string err = null;


                tmp_book = AutoMapper.Mapper.Map<Book>(book);
                tmp_book.ModifiedDate = System.DateTime.Now;

                var dao = new UserService();

                err = dao.UpdateBook(tmp_book);

                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Book/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Book/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
