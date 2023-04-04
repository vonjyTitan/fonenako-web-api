
using System;
using System.Linq;
using System.Threading.Tasks;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service;
using fonenako_service.Daos;
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

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _options = new DbContextOptionsBuilder<FonenakoDbContext>()
                .UseInMemoryDatabase(databaseName: $"{nameof(LeaseOfferDaoTest)}DB")
                .Options;

            var now = DateTime.Now;
            using var context = new FonenakoDbContext(_options);
            for(var i = 1; i<= 10; i++)
            {
                context.Add(new LeaseOffer
                {
                    LeaseOfferID = i,
                    Title = $"Offer number {i}",
                    Rooms = i,
                    MonthlyRent = 1000 + 100 * i,
                    Surface = 10 * i,
                    CreationDate = now.AddDays(i)
                });
            }
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

        [TestCase(0, 1, nameof(LeaseOffer.LeaseOfferID), TestName = "Page size 0 should make it rise an error")]
        [TestCase(10, 0, nameof(LeaseOffer.LeaseOfferID), TestName = "Page index 0 should make it rise an error")]
        [TestCase(10, 1, nameof(LeaseOffer.Carousel), TestName = "Field not orderable should make it rise an error")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown order field should make it rise an error")]
        public void RetrieveLeaseOffersByPageAsync_shloud_rise_argumentexception_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _leaseOfferDao.RetrieveLeaseOffersByPageAsync(pageSize, pageIndex, LeaseOfferFilter.Default, orderBy, Order.Asc));
        }

        [Test]
        public void RetrieveLeaseOffersByPageAsync_shloud_rise_argumentnullexception_when_called_with_filter_null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _leaseOfferDao.RetrieveLeaseOffersByPageAsync(1, 1, null, nameof(LeaseOffer.LeaseOfferID), Order.Asc));
        }

        [TestCase(5, 1, nameof(LeaseOffer.LeaseOfferID), Order.Asc, new[] { 1, 2, 3, 4, 5}, TestName = "Order by LeaseOfferID Asc")]
        [TestCase(5, 2, nameof(LeaseOffer.LeaseOfferID), Order.Asc, new[] { 6, 7, 8, 9, 10 }, TestName = "Second page Order by LeaseOfferID Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.LeaseOfferID), Order.Desc, new[] { 10, 9, 8, 7, 6 }, TestName = "Order by LeaseOfferID Desc")]
        [TestCase(5, 1, nameof(LeaseOffer.MonthlyRent), Order.Desc, new[] { 10, 9, 8, 7, 6 }, TestName = "Order by MonthlyRent Desc")]
        [TestCase(5, 1, nameof(LeaseOffer.MonthlyRent), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "Order by MonthlyRent Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.Surface), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "Order by Surface Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.Surface), Order.Desc, new[] { 10, 9, 8, 7, 6 }, TestName = "Order by Surface Desc")]
        [TestCase(5, 1, nameof(LeaseOffer.CreationDate), Order.Asc, new[] { 1, 2, 3, 4, 5 }, TestName = "Order by CreationDate Asc")]
        [TestCase(5, 1, nameof(LeaseOffer.CreationDate), Order.Desc, new[] { 10, 9, 8, 7, 6 }, TestName = "Order by CreationDate Desc")]
        [TestCase(11, 2, nameof(LeaseOffer.Surface), Order.Desc, new int[0], TestName = "Request out of range page should return empty")]
        public async Task RetrieveLeaseOffersByPageAsync_should_return_requested_page(int pageSize, int pageIndex, string orderBy, Order order, int[] expectedResultIds)
        {
            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(pageSize, pageIndex, LeaseOfferFilter.Default, orderBy, order);

            CollectionAssert.AreEqual(expectedResultIds, retrievedOffers.Select(offer => offer.LeaseOfferID));
        }

        [Test]
        public async Task RetrieveLeaseOffersByPageAsync_should_apply_the_given_filter_return_requested_page_with_correct_order()
        {
            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(2, 2, new LeaseOfferFilter{ SurfaceMin = 50d} , nameof(LeaseOffer.Surface), Order.Desc);

            CollectionAssert.AreEqual(new[] { 8, 7}, retrievedOffers.Select(offer => offer.LeaseOfferID));
        }

        [TestCaseSource(nameof(ValidFilterTestSource))]
        public async Task RetrieveLeaseOffersByPageAsync_should_apply_the_given_filter(LeaseOfferFilter filter, object[] expectedResultIds, string messageOnFail)
        {
            var retrievedOffers = await _leaseOfferDao.RetrieveLeaseOffersByPageAsync(1000, 1, filter, nameof(LeaseOffer.LeaseOfferID), Order.Asc);

            CollectionAssert.AreEqual(expectedResultIds, retrievedOffers.Select(offer => offer.LeaseOfferID).ToArray(), messageOnFail);
        }

        [Test]
        public async Task FindLeaseOfferByIdAsync_should_return_null()
        {
            var leaseOffer = await _leaseOfferDao.FindLeaseOfferByIdAsync(100);

            Assert.IsNull(leaseOffer);
        }

        [Test]
        public void FindLeaseOfferByIdAsync_should_throw_argumentexception_when_the_id_is_invalid()
        {
            Assert.ThrowsAsync<ArgumentException>(()=> _leaseOfferDao.FindLeaseOfferByIdAsync(0));
            Assert.ThrowsAsync<ArgumentException>(() => _leaseOfferDao.FindLeaseOfferByIdAsync(-1));
        }

        [Test]
        public async Task FindLeaseOfferByIdAsync_should_return_the_right_lease_offer()
        {
            var leaseOffer = await _leaseOfferDao.FindLeaseOfferByIdAsync(2);

            Assert.IsNotNull(leaseOffer);
            Assert.AreEqual(2, leaseOffer.LeaseOfferID);
        }

        private static object[] ValidFilterTestSource = new []
        {
            new object[]{ new LeaseOfferFilter { SurfaceMin = 50d }, new object[]{ 5, 6, 7, 8, 9, 10 }, "Should return all lease offers with surface greater than 49"},
            new object[]{ new LeaseOfferFilter { SurfaceMax = 50d }, new object[]{ 1, 2, 3, 4, 5}, "Should return all lease offers with surface smaller than 51" },
            new object[]{ new LeaseOfferFilter { Rooms = 5 }, new object[]{ 5 }, "Should return all lease offers with rooms equals to 5" },
            new object[]{ new LeaseOfferFilter { MonthlyRentMin = 1500d }, new object[]{ 5, 6, 7, 8, 9, 10}, "Should return all lease offers with monthly rent greater than 1499.99" },
            new object[]{ new LeaseOfferFilter { MonthlyRentMax = 1500d }, new object[]{ 1, 2, 3, 4, 5}, "Should return all lease offers with monthly rent smaller than 1500.01" }
        };
    }
}
