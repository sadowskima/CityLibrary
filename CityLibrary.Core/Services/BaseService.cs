using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLibrary.Core.Services
{
    public abstract class BaseService
    {
        protected readonly ILog Log;

        protected BaseService()
        {
            Log = LogManager.GetLogger(GetType());
        }
    }
}
