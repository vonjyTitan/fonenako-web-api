
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace fonenako_service_tests.Daos
{
    [TestFixture]
    public class LeaseOfferDaoTest
    {
        private LeaseOfferDao _leaseOfferDao;

        private FonenakoDbContext _dbContext;

        private DbContextOptions<FonenakoDbContext> _options;

        private SqliteConnection _connection;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<FonenakoDbContext>()
                .UseSqlite(_connection)
                .Options;

            var now = DateTime.Now;
            using var context = new FonenakoDbContext(_options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            for (var i = 1;  i<=10; i++)
            {
                var city = new Localisation
                {
                    LocalisationId = i,
                    Name = $"City{i}",
                    Type = LocalisationType.CIT
                };
                city.SubLocalisations.Add(new ()
                {
                    LocalisationId = i + 10,
                    Name = $"Area{i}",
                    Type = LocalisationType.ARE,
                    LeaseOffers = new List<LeaseOffer>()
                    {
                        new LeaseOffer
                        {
                            LeaseOfferID = i,
                            Title = $"Offer number {i}",
                            Rooms = i,
                            MonthlyRent = 1000 + 100 * i,
                            Surface = 10 * i,
                            CreationDate = now.AddDays(i),
                            Description = $"Offer {i} description"
                        }
                    }
                });

                context.Add(city);
            }

            var cityException = new Localisation
            {
                LocalisationId = 21,
                Type = LocalisationType.CIT,
                Name = "CityForException",
                LeaseOffers = new List<LeaseOffer> {
                    new LeaseOffer
                    {
                        LeaseOfferID = 11,
                        Title = $"Offer number 11",
                        Rooms = 11,
                        MonthlyRent = 1000 + 100 * 11,
                        Surface = 10 * 11,
                        CreationDate = now.AddDays(11),
                        Description = $"Offer 11 description"
                    }
                }

            };

            context.Add(cityException);

            context.SaveChanges();
        }


        [SetUp]
        public void Setup()
        {
            _dbContext = new FonenakoDbContext(_options);
            _leaseOfferDao = new LeaseOfferDao(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _connection.Close();
        }

        [TestCase(0, 1, nameof(LeaseOffer.LeaseOfferID), TestName = "Page size 0 should make RetrieveLeaseOffersByPageAsync rise an error")]
        [TestCase(10, 0, nameof(LeaseOffer.LeaseOfferID), TestName = "Page index 0 should make RetrieveLeaseOffersByPageAsync rise an error")]
        [TestCase(10, 1, nameof(LeaseOffer.Carousel), TestName = "Field not orderable should make RetrieveLeaseOffersByPageAsync rise an error")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown order field should make RetrieveLeaseOffersByPageAsync rise an error")]
        public void RetrieveLeaseOffersByPageAsync_shloud_rise_argumentexception_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _leaseOfferDao.RetrieveLeaseOffersByPageAsync(pageSize, pageIndex, LeaseOfferFilter.Default, orderBy, Order.Asc));
        }

        [Test]
        public void RetrieveLeaseOffersByPageAsync_shloud_rise_argumentnullexception_when_called_with_filter_null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _leaseOfferDao.RetrieveLeaseOffersByPageAsync(1, 1, null, nameof(LeaseOffer.LeaseOfferID), Order.Asc));
        }

        [TestCase(5, 1, nameof(LeaseOffer.LeaseOfferID), Order.Asc, new[] { 1, 2, 3, 4, 5}, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers order by LeaseOfferID Asc")]
        [TestCase(5, 2, nameof(LeaseOffer.LeaseOfferID), Order.Asc, new[] { 6, 7, 8, 9, 10 }, TestName = "RetrieveLeaseOffersByPageAsync should return Second of lease offers page Order by LeaseOfferID Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.LeaseOfferID), Order.Desc, new[] { 11, 10, 9, 8, 7 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by LeaseOfferID Desc")]
        [TestCase(5, 1, nameof(LeaseOffer.MonthlyRent), Order.Desc, new[] { 11, 10, 9, 8, 7 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by MonthlyRent Desc")]
        [TestCase(5, 1, nameof(LeaseOffer.MonthlyRent), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by MonthlyRent Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.Surface), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by Surface Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.Surface), Order.Desc, new[] { 11, 10, 9, 8, 7 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by Surface Desc")]
        [TestCase(5, 1, nameof(LeaseOffer.CreationDate), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by CreationDate Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.CreationDate), Order.Desc, new[] { 11, 10, 9, 8, 7 }, TestName = "RetrieveLeaseOffersByPageAsync should return lease offers Order by CreationDate Desc")]
        [TestCase(11, 2, nameof(LeaseOffer.Surface), Order.Desc, new int[0], TestName = "RetrieveLeaseOffersByPageAsync should return empty when the requested page is out of range")]
        public async Task RetrieveLeaseOffersByPageAsync_should_return_requested_page(int pageSize, int pageIndex, string orderBy, Order order, int[] expectedResultIds)
        {
            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(pageSize, pageIndex, LeaseOfferFilter.Default, orderBy, order);

            CollectionAssert.AreEqual(expectedResultIds, retrievedOffers.Select(offer => offer.LeaseOfferID));
        }

        [Test]
        public async Task RetrieveLeaseOffersByPageAsync_should_apply_the_given_filter_and_return_requested_page_with_correct_order()
        {
            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(2, 2, new LeaseOfferFilter{ SurfaceMin = 50d} , nameof(LeaseOffer.Surface), Order.Desc);

            CollectionAssert.AreEqual(new[] { 9, 8}, retrievedOffers.Select(offer => offer.LeaseOfferID));
        }

        [TestCaseSource(nameof(ValidFilterTestSource))]
        public async Task RetrieveLeaseOffersByPageAsync_should_apply_the_given_filter(LeaseOfferFilter filter, int[] expectedResultIds, string messageOnFail)
        {
            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(1000, 1, filter, nameof(LeaseOffer.LeaseOfferID), Order.Asc);

            CollectionAssert.AreEqual(expectedResultIds, retrievedOffers.Select(offer => offer.LeaseOfferID).ToArray(), messageOnFail);
        }

        private static object[] ValidFilterTestSource = new[]
        {
            new object[]{ new LeaseOfferFilter { SurfaceMin = 50d }, new []{ 5, 6, 7, 8, 9, 10, 11 }, "RetrieveLeaseOffersByPageAsync Should return all lease offers with surface greater than 49"},
            new object[]{ new LeaseOfferFilter { SurfaceMax = 50d }, new []{ 1, 2, 3, 4, 5}, "RetrieveLeaseOffersByPageAsync Should return all lease offers with surface smaller than 51" },
            new object[]{ new LeaseOfferFilter { Rooms = new[] { 4, 5 } }, new []{ 4, 5 }, "RetrieveLeaseOffersByPageAsync Should return all lease offers with rooms in [4 and 5]" },
            new object[]{ new LeaseOfferFilter { MonthlyRentMin = 1500d }, new []{ 5, 6, 7, 8, 9, 10, 11}, "RetrieveLeaseOffersByPageAsync Should return all lease offers with monthly rent greater than 1499.99" },
            new object[]{ new LeaseOfferFilter { MonthlyRentMax = 1500d }, new []{ 1, 2, 3, 4, 5}, "RetrieveLeaseOffersByPageAsync Should return all lease offers with monthly rent smaller than 1500.01" },
            new object[]{ new LeaseOfferFilter { Localisations = new[] { 11, 12 } }, new []{ 1, 2}, "RetrieveLeaseOffersByPageAsync Should return all lease offers present in areas : [11 and 12]" },
            new object[]{ new LeaseOfferFilter { Localisations = new[] { 1, 2 } }, new []{ 1, 2}, "RetrieveLeaseOffersByPageAsync Should return all lease offers present in the areas in the cities : [1 and 2]" },
            new object[]{ new LeaseOfferFilter { Localisations = new[] { 21 } }, new []{ 11 }, "RetrieveLeaseOffersByPageAsync Should return all lease offers present in the city : [21]" }
        };

        [Test]
        public async Task FindLeaseOfferDetailsByIdAsync_should_return_null()
        {
            var leaseOffer = await _leaseOfferDao.FindLeaseOfferDetailsByIdAsync(100);

            Assert.IsNull(leaseOffer);
        }

        [Test]
        public void FindLeaseOfferDetailsByIdAsync_should_throw_argumentexception_when_the_id_is_invalid()
        {
            Assert.ThrowsAsync<ArgumentException>(()=> _leaseOfferDao.FindLeaseOfferDetailsByIdAsync(0));
            Assert.ThrowsAsync<ArgumentException>(() => _leaseOfferDao.FindLeaseOfferDetailsByIdAsync(-1));
        }

        [Test]
        public async Task FindLeaseOfferDetailsByIdAsync_should_return_the_right_lease_offer()
        {
            var leaseOffer = await _leaseOfferDao.FindLeaseOfferDetailsByIdAsync(2);

            Assert.IsNotNull(leaseOffer);
            Assert.AreEqual(2, leaseOffer.LeaseOfferID);
        }

        [Test]
        public void CountLeaseOffersAsync_shloud_rise_argumentnullexception_when_called_with_filter_null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _leaseOfferDao.CountLeaseOffersAsync(null));
        }

        [TestCaseSource(nameof(ValidFilterCountTestSource))]
        public async Task CountLeaseOffersAsync_should_apply_the_given_filter(LeaseOfferFilter filter, int expectedCount, string messageOnFail)
        {
            var retrievedOfferCount = await _leaseOfferDao.CountLeaseOffersAsync(filter);

            Assert.AreEqual(expectedCount, retrievedOfferCount, messageOnFail);
        }

        private static object[] ValidFilterCountTestSource = new[]
        {
            new object[]{ new LeaseOfferFilter { SurfaceMin = 50d }, 7, "CountLeaseOffersAsync Should return size of lease offers with surface greater than 49"},
            new object[]{ new LeaseOfferFilter { SurfaceMax = 50d }, 5, "CountLeaseOffersAsync Should return size of lease offers with surface smaller than 51" },
            new object[]{ new LeaseOfferFilter { Rooms = new[] { 4, 5 } }, 2, "CountLeaseOffersAsync Should return size of lease offers with rooms in [4 and 5]" },
            new object[]{ new LeaseOfferFilter { MonthlyRentMin = 1500d }, 7, "CountLeaseOffersAsync Should return size of lease offers with monthly rent greater than 1499.99" },
            new object[]{ new LeaseOfferFilter { MonthlyRentMax = 1500d }, 5, "CountLeaseOffersAsync Should return size of lease offers with monthly rent smaller than 1500.01" },
            new object[]{ new LeaseOfferFilter { Localisations = new[] { 11, 12 } }, 2, "CountLeaseOffersAsync Should return size of lease offers present in areas : [11 and 12]" },
            new object[]{ new LeaseOfferFilter { Localisations = new[] { 1, 2 } }, 2, "CountLeaseOffersAsync Should return size of lease offers present in the areas in the cities : [1 and 2]" },
            new object[]{ new LeaseOfferFilter { Localisations = new[] { 21 } }, 1, "CountLeaseOffersAsync Should return size of lease offers present in the city : [21]" }
        };
    }
}
