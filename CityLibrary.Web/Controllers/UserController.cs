using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CityLibrary.Core.Dao;
using CityLibrary.Web.Models;
using CityLibrary.Web.ViewModels;

namespace CityLibrary.Web.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Member/

        static UserController()
        {
            AutoMapper.Mapper.CreateMap<User, UserModel>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserID));
            AutoMapper.Mapper.CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserID));//NIE UMIE TEGO MAPOWAC!!!!!


            AutoMapper.Mapper.CreateMap<User, UserLiteModel>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)))
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserID));
        }

        public ActionResult Index()
        {
            var userService = new UserService();
            List<User> users = userService.GetUsers();

            //var firstUser = users.FirstOrDefault(u => u.FirstName == "Czesław");

            // nie moze byc nulla
            //var single = users.SingleOrDefault(u => u.UserID == 1);

            //if (single != null)
            //{
            //    var email = single.Email;
            //}

            //int sss = users.Where(u => u.FirstName.ToUpper().Contains("CZE")).Sum(u => u.LastName.Length);

            //mapowanie bez automappera
            var members = users
                .Select(u =>
                    new UserViewModel
                    {
                        UserId = u.UserID,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        BirthDate = u.BirthDate,
                        Phone = u.Phone,
                        ModifiedDate = u.ModifiedDate,
                        Email = u.Email,
                        IsActive = u.IsActive,
                        HasBooks = u.HasBooks,
                    }
            ).ToList();

            return View(members);
        }

        //
        // GET: /Member/Details/5

        public ActionResult Details(int id)
        {
            var us = new UserService();
            var user = us.GetUser(id);
            var vm = new UserViewModel();

           // vm = AutoMapper.Mapper.Map<User,UserViewModel>(user);  !@#$%^&
            // wiem że zrobiłem na około, ale nie potrafię zrozumieć dlaczego automaper nie poradził...
            vm.UserId = user.UserID;
            vm.HasBooks = user.HasBooks;
            vm.IsActive = user.IsActive;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.AddDate = user.AddDate;
            vm.ModifiedDate = user.ModifiedDate;
            vm.BirthDate = user.BirthDate;
            vm.Borrows = user.Borrows;
            vm.Email = user.Email;

            return View(vm);
        }

        //
        // GET: /Member/Create

        public ActionResult Create()
        {
            var model = new UserModel()
            {
                AddDate = DateTime.Now,
                BirthDate = DateTime.Today.AddYears(-5),
                IsActive = true
            };

            return View(model);
        }

        //
        // POST: /Member/Create

        [HttpPost]
        public ActionResult Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.BirthDate > DateTime.Now)
                {
                    ModelState.AddModelError("BirthDate", "Jesteś z przyszłości?");
                    return View(user);
                }


                try
                {
                    var dao = new UserService();

                    var u = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                        Email = user.Email,
                        Phone = user.Phone,
                        AddDate = user.AddDate,
                        IsActive = user.IsActive
                    };

                    var err = dao.CreateUser(u);
                    if (err != null)
                    {
                        ModelState.AddModelError("", "Wystąpił problem podczas dodawania nowego użytkownika.");
                        return View(user);
                    }


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }



        }

        [HttpPost]
        public JsonResult GetUsersJson()
        {
            var dao = new UserService();

            var users = dao.GetUsers();
            var listModel = AutoMapper.Mapper.Map<List<User>, List<UserLiteModel>>(users);

            listModel.Insert(0, new UserLiteModel() { UserId = 0, FullName = "Wybierz użytkownika" });

            return this.Json(new { Items = listModel });
        }

        public ActionResult Edit(int id)
        {


            var dao = new UserService();
            var user = dao.GetUser(id);

            UserModel model = AutoMapper.Mapper.Map<User, UserModel>(user);

            //var model = new UserModel()
            //{
            //    UserId = singleUser.UserID,
            //    FirstName = singleUser.FirstName,
            //    LastName = singleUser.LastName,
            //    Phone = singleUser.Phone,
            //    Email = singleUser.Email,
            //    AddDate = singleUser.AddDate,
            //    ModifiedDate = singleUser.ModifiedDate,
            //    BirthDate = singleUser.BirthDate,
            //    IsActive = singleUser.IsActive,
            //};

            return View(model);
            // return View();
        }

        //
        // POST: /Member/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, UserModel user)
        {

            if (ModelState.IsValid)
            {
                if (user.BirthDate > DateTime.Now)
                {
                    ModelState.AddModelError("BirthDate", "Jesteś z przyszłości?");
                    return View(user);
                }


                try
                {
                    var dao = new UserService();

                    var u = new User
                    {
                        UserID = id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                        Email = user.Email,
                        Phone = user.Phone,
                        AddDate = user.AddDate,
                        ModifiedDate = DateTime.Now,
                        IsActive = user.IsActive
                    };

                    var err = dao.UpdateUser(u);
                    if (err != null)
                    {
                        ModelState.AddModelError("", "Wystąpił problem podczas dodawania nowego użytkownika.");
                        return View(user);
                    }


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }

            //try
            //{
            //    // TODO: Add update logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //
        // GET: /Member/Delete/5

        public ActionResult Delete(int id)
        {

            var dao = new UserService();
            var users = dao.GetUsers();
            var singleUser = users.SingleOrDefault(u => u.UserID == id);

            var model = new UserModel()
            {
                UserId = singleUser.UserID,
                FirstName = singleUser.FirstName,
                LastName = singleUser.LastName,
                Phone = singleUser.Phone,
                Email = singleUser.Email,
                AddDate = singleUser.AddDate,
                ModifiedDate = singleUser.ModifiedDate,
                BirthDate = singleUser.BirthDate,
                IsActive = singleUser.IsActive,
            };

            return View(model);
        }

        //
        // POST: /Member/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, UserModel user)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var dao = new UserService();

                    var u = new User
                    {
                        UserID = id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate,
                        Email = user.Email,
                        Phone = user.Phone,
                        AddDate = user.AddDate,
                        ModifiedDate = DateTime.Now,
                        IsActive = false
                    };

                    var err = dao.UpdateUser(u);
                    if (err != null)
                    {
                        ModelState.AddModelError("", "Wystąpił problem podczas usuwania użytkownika.");
                        return View(user);
                    }


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }



        [HttpPost]
        public string DeleteModal(int id)
        {

            var dao = new UserService();

            if (dao.UserHasBooks(id) == false)
            {
                var err = dao.DeleteUser(id);
                return "ok";
            }
            else
            {
                return "nie ok - użytkownik posiada nasze książki";
            }


        }

    }
}
