using CityLibrary.Core.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLibrary.Core.Services
{
    public class BorrowService : BaseService
    {
        public List<Book> GetBooksToBorrow()
        {
            var context = new CityLibraryEntities();

            try
            {
                var query = (from b in context.Books
                             join br in context.Borrows on b.BookId equals br.BookId
                             into gr
                             from b2 in gr.DefaultIfEmpty()
                             where b2 == null
                             select b);

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


    }
}
