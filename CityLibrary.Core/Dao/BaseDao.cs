using log4net;

namespace CityLibrary.Core.Dao
{
    public abstract class BaseDao
    {
        protected readonly ILog Log;

        protected BaseDao()
        {
            Log = LogManager.GetLogger(GetType());
        }
    }
}
