using CityLibrary.Core.Dao;
using CityLibrary.Core.Services;
using CityLibrary.Core.Util;
using CityLibrary.Web.Models;
using CityLibrary.Web.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CityLibrary.Web.Controllers
{
    public class BorrowController : Controller
    {
        static BorrowController()
        {
            AutoMapper.Mapper.CreateMap<BookLite, BookLiteModel>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => string.Format("({0}) {1}", src.LeftCount, src.Title)));
            AutoMapper.Mapper.CreateMap<Book, BookModel>();

            AutoMapper.Mapper.CreateMap<Borrow, BorrowLiteModel>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.Author, opts => opts.MapFrom(src => src.Book.Author))
                .ForMember(dest => dest.UserFullName, opts => opts.MapFrom(src => string.Format("{0} {1}", src.User.FirstName, src.User.LastName)));

            AutoMapper.Mapper.CreateMap<User, UserLiteModel>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)))
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserID));


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

            var booksLite = AutoMapper.Mapper.Map<List<BookLite>, List<BookLiteModel>>(books);

            return Json(new { Books = booksLite });
        }

        [HttpPost]
        public JsonResult GetBorrows(SearchCriteria[] searchCriteria, SortOrder[] sort, int page, int pageSize)
        {

            var dao = new BorrowService();

            int count;
            List<Borrow> list = dao.GetBorrows(searchCriteria, sort, page, pageSize, out count);

            var borrows = AutoMapper.Mapper.Map<List<Borrow>, List<BorrowLiteModel>>(list);

            return Json(new { Borrows = borrows, Count = count });
        }

        [HttpPost]
        public JsonResult OrderBooks(int userId, int[] booksToBorrow)
        {
            var borrowService = new BorrowService();

            bool result = borrowService.OrderBooks(userId, booksToBorrow);

            return Json(new { Success = result });
        }

        [HttpPost]
        public JsonResult RemoveBorrow(int id)
        {
            var service = new BorrowService();

            bool result = service.RemoveBorrow(id);

            return Json(new { Success = result });
        }
        //RemoveBorrow
    }
}
