using CityLibrary.Core.Dao;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using CityLibrary.Core.Util;

namespace CityLibrary.Core.Services
{
    public class BorrowService : BaseService
    {
        public List<BookLite> GetBooksToBorrow()
        {
            var context = new CityLibraryEntities();

            try
            {
                // Zapytanie składa się z dwóch połączonych

                // 1. Pobiera listę książek które nigdy nie zostały wypżyczone (br.BorrowId is null) oraz tych które nie zostały zwrócone (br.IsReturned = 0)
                //    CASE WHEN br.IsReturned IS NULL THEN b.[Count] ELSE b.[Count] - Count(*) - Jeśli nigdy nie wypożyczona pobierz liczbę książek z właściwośći książki
                //    Jeśli jest chociaż jedna wypożyczona to zwróc różnicę (Dostępne - Wypożyczone)
                //    (Nie obsługuje przypadku gdy wszystkie zwrócone)
                //(SELECT b.BookId, b.Title, br.IsReturned, CASE WHEN br.IsReturned IS NULL THEN b.[Count] ELSE b.[Count] - Count(*) END  as DoWypozyczenia
                //FROM 
                //Book b LEFT OUTER JOIN
                //Borrow br ON b.BookId = br.BookId
                //WHERE br.BorrowId is null OR br.IsReturned = 0
                //GROUP BY b.BookId, b.Title, b.[Count], br.IsReturned

                // 2. Przypadek gdy wszystkie książki o określonym BookId są zwócone 
                //    Pobieram listę książek zwróconych (IsReturned = 1) oraz wstawiam liczbę książek z właściwości ksiazki Book.Count
                //SELECT b.BookId, b.Title, br.IsReturned, b.[Count]  as DoWypozyczenia
                //FROM 
                //Book b LEFT OUTER JOIN
                //Borrow br ON b.BookId = br.BookId
                //WHERE br.IsReturned = 1

                // 3. Grupowanie po BookId oraz Title
                //SELECT tb.BookId, tb.Title, Min(tb.DoWypozyczenia) FROM
                // ... SELECT_1 UNION SELECT_2 ...
                //GROUP BY tb.BookId, tb.Title
                //HAVING Min(tb.DoWypozyczenia) > 0

                // Cały SQL
                //SELECT tb.BookId, tb.Title, Min(tb.DoWypozyczenia) FROM
                //(SELECT b.BookId, b.Title, br.IsReturned, CASE WHEN br.IsReturned IS NULL THEN b.[Count] ELSE b.[Count] - Count(*) END  as DoWypozyczenia
                //FROM 
                //Book b LEFT OUTER JOIN
                //Borrow br ON b.BookId = br.BookId
                //WHERE br.BorrowId is null OR br.IsReturned = 0
                //GROUP BY b.BookId, b.Title, b.[Count], br.IsReturned
                //UNION
                //SELECT b.BookId, b.Title, br.IsReturned, b.[Count]  as DoWypozyczenia
                //FROM 
                //Book b LEFT OUTER JOIN
                //Borrow br ON b.BookId = br.BookId
                //WHERE br.IsReturned = 1
                //GROUP BY b.BookId, b.Title, b.[Count], br.IsReturned) as tb
                //GROUP BY tb.BookId, tb.Title
                //HAVING Min(tb.DoWypozyczenia) > 0

                // Wygenerowane przy pomocy http://www.sqltolinq.com/
                // Dopracowane w http://www.linqpad.net/

                var query = (
                    from tb in (
                        (
                            from b in context.Books
                            join br in context.Borrows on b.BookId equals br.BookId into br_join
                            from brj in br_join.DefaultIfEmpty()
                            where 
                                brj == null ||
                                brj.IsReturned == false
                            group new {b, brj} by new
                            {
                                b.BookId,
                                b.Title,
                                b.Count,
                                brj.IsReturned
                            }
                            into g
                            select new
                            {
                                BookId = (Int32?) g.Key.BookId,
                                g.Key.Title,
                                IsReturned = (Boolean?) g.Key.IsReturned,
                                LeftCount = g.Key.IsReturned == null ? (Int64) g.Key.Count : (g.Key.Count - g.Count())
                            }
                        ).Union(
                            from b in context.Books
                            join br in context.Borrows on b.BookId equals br.BookId into br_join
                            from brj in br_join.DefaultIfEmpty()
                            where
                                brj.IsReturned == true
                            group new {b, brj} by new
                            {
                                b.BookId,
                                b.Title,
                                b.Count,
                                brj.IsReturned
                            }
                            into g
                            select new
                            {
                                BookId = (Int32?) g.Key.BookId,
                                g.Key.Title,
                                IsReturned = (Boolean?) g.Key.IsReturned,
                                LeftCount = (Int64)g.Key.Count
                            }
                        )
                    )
                    group tb by new
                    {
                        tb.BookId,
                        tb.Title
                    }
                    into g
                        where g.Min(p => p.LeftCount) > 0
                    select new BookLite
                    {
                        BookId = g.Key.BookId.Value,
                        Title = g.Key.Title,
                        LeftCount = (int) g.Min(p => p.LeftCount)
                    });

                //var sql = ((System.Data.Entity.Infrastructure.DbQuery<BookLite>)query).ToString();

                return query.ToList();
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



        public bool OrderBooks(int userId, int[] booksToBorrow)
        {
            var context = new CityLibraryEntities();

            try
            {
                foreach (var bookId in booksToBorrow)
                {
                    var borrow = new Borrow
                    {
                        FromDate = DateTime.Today,
                        ToDate = DateTime.Today.AddDays(7),
                        IsReturned = false,
                        BookId = bookId,
                        UserId = userId
                    };

                    context.Borrows.Add(borrow);
                }

                context.SaveChanges();

                return true;
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
        }

        public List<Borrow> GetBorrows(SearchCriteria[] searchCriteria, SortOrder[] sort, int page, int pageSize, out int count)
        {
            var context = new CityLibraryEntities();

            List<Borrow> borrows = null;

            try
            {
                count = context.Borrows.Count();

                // Linq
                IQueryable<Borrow> query = context.Borrows;

                // Search
                if (searchCriteria != null && searchCriteria.Length > 0)
                {
                    foreach (var sc in searchCriteria)
                    {
                        //query = query.Where(b => b.Author == sc.Value)
                    }
                }

                query = query.Where(b => b.IsReturned == false);

                // Sorting
                if (sort != null && sort.Length > 0)
                {
                    //foreach (SortOrder sc in sort)
                    //{
                        
                    //}
                }
                else
                {
                    query = query.OrderByDescending(b => b.BorrowId);
                }

                query = query.Include(u => u.Book).Include(u => u.User);

                // Pagging
                query = query.Skip(page * pageSize).Take(pageSize);

                // Retrieve from database
                borrows = query.ToList();
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

            return borrows;
        }

        public bool RemoveBorrow(int borrowId)
        {
            var context = new CityLibraryEntities();

            try
            {
                var borrow = context.Borrows.SingleOrDefault(b => b.BorrowId == borrowId);

                if (borrow != null)
                {
                    borrow.IsReturned = true;
                    context.SaveChanges();
                }

                return true;
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
        }
    }
}
