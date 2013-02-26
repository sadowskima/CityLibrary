using CityLibrary.Core.Dao;
using CityLibrary.Core.Services;
using CityLibrary.Web.Models;
using CityLibrary.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityLibrary.Web.Controllers
{
    public class BorrowController : Controller
    {
        static BorrowController()
        {
            AutoMapper.Mapper.CreateMap<Book, BookLiteModel>();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewBorrowView()
        {
            var vm = new BorrowViewModel();

            return PartialView("_NewBorrow", vm);
        }

        [HttpPost]
        public ActionResult AddBorrow(BookViewModel vm)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetBooksToBorrow()
        {
            var borrowService = new BorrowService();

            var books = borrowService.GetBooksToBorrow();

            var booksLite = AutoMapper.Mapper.Map<List<Book>, List<BookLiteModel>>(books);

            return Json(new { Books = booksLite });
        }
    }
}
