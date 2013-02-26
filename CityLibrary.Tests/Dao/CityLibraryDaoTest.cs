using CityLibrary.Core.Dao;
using NUnit.Framework;

namespace CityLibrary.Tests.Dao
{
    [TestFixture]
    public class CityLibraryDaoTest
    {
        [Test]
        public void GetUsersTest()
        {
            var dao = new UserService();
            var users = dao.GetUsers();
            Assert.IsNotNull(users);
        }
    }
}
