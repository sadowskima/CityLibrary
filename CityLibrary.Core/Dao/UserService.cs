using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CityLibrary.Core.Util;

namespace CityLibrary.Core.Dao
{
    /// <summary>
    /// City Library data access object.
    /// </summary>
    public class UserService : BaseDao
    {
        static UserService()
        {
            AutoMapper.Mapper.CreateMap<User, User>();
            AutoMapper.Mapper.CreateMap<Borrow, Borrow>();
        }

        public User GetUser(int id)
        {
            var context = new CityLibraryEntities();

            try
            {

                var user = context.Users
                    .Where(u => u.UserID == id)
                    .Include(u => u.Borrows).SingleOrDefault();

                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<User> GetUsers()
        {
            List<User> users = null;

            var context = new CityLibraryEntities();

            try
            {
                var query = (
                    from u in context.Users
                    where u.IsActive
                    orderby u.LastName
                    select u
                ).Include(u => u.Borrows);

                //var sql = ((System.Data.Entity.Infrastructure.DbQuery<User>)query).ToString();

                users = query.ToList<User>();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return users;
        }

        // do ulepszenia

        public bool UserHasBooks(int id)
        {
            List<Borrow> notReturnedBorrows = null; 

            var context = new CityLibraryEntities();

            try
            {
                notReturnedBorrows = (
                    from b in context.Borrows
                    where ( b.UserId == id && b.IsReturned == false)
                    orderby b.BookId
                    select b
                ).ToList();




            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return false;
            
            }
            finally
            {
                context.Dispose();
            }

            if (notReturnedBorrows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public string CreateUser(User user)
        {
            string err = null;

            var context = new CityLibraryEntities();

            try
            {
                //  throw new Exception("aaaaaaaaaaaaa");


                context.Users.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                err = ex.Message;
            }

            return err;
        }

        public string UpdateUser(User user)
        {
            string err = null;

            var context = new CityLibraryEntities();

            try
            {
                var currentUser = context.Users.SingleOrDefault(u => u.UserID == user.UserID);
                if (currentUser != null)
                {
                    currentUser.FirstName = user.FirstName;
                    currentUser.LastName = user.LastName;

                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                err = ex.Message;
            }

            return err;
        }



        public string DeleteUser(int id)
        {
            string err = null;

            var context = new CityLibraryEntities();

            try
            {
                var currentUser = context.Users.SingleOrDefault(u => u.UserID == id);
                if (currentUser != null)
                {
                    currentUser.IsActive = false;

                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                err = ex.Message;
            }

            return err;
        }


        public List<Book> GetBooks(SearchCriteria[] searchCriterias, SortOrder[] sortOrders, int page, int pageSize, out int count)
        {
            var context = new CityLibraryEntities();
            List<Book> books = null; 


            try
            {
                count = context.Books.Count();

                // Linq
                IQueryable<Book> query = context.Books;

                // Search
                if (searchCriterias != null && searchCriterias.Length > 0)
                {
                    foreach (var sc in searchCriterias)
                    {
                        //query = query.Where(b => b.Author == sc.Value)
                    }
                }

                // Sorting
                if (sortOrders != null)
                {
                    // logika
                }
                else
                {
                    query = query.OrderBy(b => b.BookId);
                }

                // Pagging
                //query = query.Skip(page*pageSize).Take(page);
           
                // Retrieve from database
                books = query.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                context.Dispose();
            }
            
            return books;
        }

        public string CreateBook(Book book)
        {
            string err = null;

            var context = new CityLibraryEntities();

            try
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                err = ex.Message;
            }

            return err;
        }



        public List<global::CityLibrary.Core.Dao.DictBookGenre> GetGenres()
        {
            var context = new CityLibraryEntities();
            List<DictBookGenre> genres = null;


            try
            {

                // Linq
                IQueryable<DictBookGenre> query = context.DictBookGenres;
                // Sorting
                    query = query.OrderBy(b => b.BookGenreId);
                // Pagging
                //query = query.Skip(page*pageSize).Take(page);

                // Retrieve from database
                genres = query.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                context.Dispose();
            }

            return genres;
        }


        public Book GetBook(int id)
        {
            var context = new CityLibraryEntities();

            try
            {
                return context.Books.SingleOrDefault(b => b.BookId == id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<DictBookGenre> GetGenreList()
        {
            var context = new CityLibraryEntities();

            try
            {
                return context.DictBookGenres.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public string UpdateBook(Book book)
        {
            string err = null;

            var context = new CityLibraryEntities();

            try
            {
                var currentBook = context.Books.SingleOrDefault(b => b.BookId == book.BookId);
                AutoMapper.Mapper.CreateMap<Book, Book>();
                if (currentBook != null)
                {
                    currentBook = AutoMapper.Mapper.Map<Book, Book>(book);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                err = ex.Message;
            }

            return err;
        }
    }
}
