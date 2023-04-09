
using System;
using System.Linq;
using System.Threading.Tasks;
using fonenako.DatabaseContexts;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace finenako_service_tests.Daos
{
    [TestFixture]
    public class CityDaoTest
    {
        private CityDao _cityDao;

        private FonenakoDbContext _dbContext;

        private DbContextOptions<FonenakoDbContext> _options;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _options = new DbContextOptionsBuilder<FonenakoDbContext>()
                .UseInMemoryDatabase(databaseName: $"{nameof(CityDao)}DB")
                .Options;

            var now = DateTime.Now;
            using var context = new FonenakoDbContext(_options);

            for (var i = 1; i <= 20; i++)
            {
                var city = new City
                {
                    CityId = i,
                    Name = $"City{i}"
                };
                city.Areas.Add(new()
                {
                    AreaId = i,
                    Name = $"Area{i}"
                });

                context.Add(city);
            }

            context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new FonenakoDbContext(_options);
            _cityDao = new CityDao(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [TestCase(0, 1, nameof(City.Name), TestName = "Page size 0 should make RetrieveCitiesByPageAsync rises an error")]
        [TestCase(10, 0, nameof(City.Name), TestName = "Page index 0 should make RetrieveCitiesByPageAsync rises an error")]
        [TestCase(10, 1, nameof(City.CityId), TestName = "Field not orderable should make RetrieveCitiesByPageAsync rises an error")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown order field should make RetrieveCitiesByPageAsync rises an error")]
        public void RetrieveCitiesByPageAsync_shloud_rise_argumentexception_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _cityDao.RetrieveCitiesByPageAsync(pageSize, pageIndex, CityFilter.Default, orderBy, Order.Asc));
        }

        [Test]
        public void RetrieveCitiesByPageAsync_shloud_rise_argumentnullexception_when_called_with_filter_null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _cityDao.RetrieveCitiesByPageAsync(1, 1, null, nameof(City.Name), Order.Asc));
        }

        [TestCase(5, 1, nameof(City.Name), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "Order by name Asc")]
        [TestCase(5, 2, nameof(City.Name), Order.Asc, new[] { 6, 7, 8, 9, 10 }, TestName = "Second page Order by asc Asc")]
        [TestCase(11, 2, nameof(City.Name), Order.Desc, new int[0], TestName = "Request out of range page should return empty")]
        public async Task RetrieveCitiesByPageAsync_should_return_requested_page(int pageSize, int pageIndex, string orderBy, Order order, int[] expectedResultIds)
        {
            var retrievedCities = await _cityDao.RetrieveCitiesByPageAsync(pageSize, pageIndex, CityFilter.Default, orderBy, order);

            CollectionAssert.AreEqual(expectedResultIds, retrievedCities.Select(city => city.CityId));
        }

        public async Task RetrieveCitiesByPageAsync_should_apply_the_given_filter_without_carrying_case()
        {
            var retrievedCities = await _cityDao.RetrieveCitiesByPageAsync(1000, 1, new CityFilter()
            {
                Name = "ITy1"
            }, nameof(City.Name), Order.Asc);

            CollectionAssert.AreEqual(new[] { 1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19}, retrievedCities.Select(city => city.CityId).ToArray());
        }

        [Test]
        public void CountCitiesAsync_shloud_rise_argumentnullexception_when_called_with_filter_null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _cityDao.CountCitiesAsync(null));
        }

        [Test]
        public async Task CountCitiesAsync_should_apply_the_given_filter_without_carrying_case()
        {
            var retrievedOfferCount = await _cityDao.CountCitiesAsync(new CityFilter
            {
                Name = "ITy1"
            });

            Assert.AreEqual(11, retrievedOfferCount);
        }
    }
}