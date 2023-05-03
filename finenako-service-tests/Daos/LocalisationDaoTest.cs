using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fonenako.DatabaseContexts;
using fonenako_service.Daos;
using fonenako_service.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace finenako_service_tests.Daos
{
    [TestFixture]
    public class LocalisationDaoTest
    {
        private LocalisationDao _localisationDao;

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

            using var context = new FonenakoDbContext(_options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            for (var i = 1; i < 5; i++)
            {
                var city = new Localisation
                {
                    LocalisationId = i,
                    Name = $"City{i}",
                    Type = LocalisationType.CIT,
                    SubLocalisations = new List<Localisation>
                    {
                        new()
                        {
                            LocalisationId = i + 4,
                            Name = $"Area{i}",
                            Type = LocalisationType.ARE
                        }
                    }
                };

                context.Add(city);
            }

            context.SaveChanges();
        }


        [SetUp]
        public void Setup()
        {
            _dbContext = new FonenakoDbContext(_options);
            _localisationDao = new LocalisationDao(_dbContext);
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

        [TestCase(0, 1, nameof(Localisation.Name), TestName = "Page size 0 should make SearchLocalisationsAsync rise an error")]
        [TestCase(10, 0, nameof(Localisation.Name), TestName = "Page index 0 should make SearchLocalisationsAsync rise an error")]
        [TestCase(10, 1, nameof(Localisation.LocalisationId), TestName = "Field not sortable should make SearchLocalisationsAsync rise an error")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown order field should make SearchLocalisationsAsync rise an error")]
        public void SearchLocalisationsAsync_shloud_rise_argumentexception_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _localisationDao.SearchLocalisationsAsync(pageSize, pageIndex, null, orderBy, Order.Asc));
        }


        [TestCase(4, 1, nameof(Localisation.Name), Order.Asc, new[] { 5, 6, 7, 8 }, TestName = "SearchLocalisationsAsync should return localisations order by Name Asc")]
        [TestCase(4, 1, nameof(Localisation.Name), Order.Desc, new[] { 4, 3, 2, 1 }, TestName = "SearchLocalisationsAsync should return localisations order by Name Desc")]
        [TestCase(4, 2, nameof(Localisation.Name), Order.Asc, new[] { 1, 2, 3, 4 }, TestName = "SearchLocalisationsAsync should return Second page of localisations Order by Name Asc")]
        [TestCase(4, 1, nameof(Localisation.Type), Order.Asc, new[] { 1, 2, 3, 4 }, TestName = "SearchLocalisationsAsync should return localisations order by Type Asc")]
        [TestCase(4, 1, nameof(Localisation.Type), Order.Desc, new[] { 5, 6, 7, 8 }, TestName = "SearchLocalisationsAsync should return localisations order by Type Desc")]
        [TestCase(11, 2, nameof(Localisation.Name), Order.Desc, new int[0], TestName = "SearchLocalisationsAsync should return empty when the requested page is out of range")]
        public async Task SearchLocalisationsAsync_should_return_requested_page(int pageSize, int pageIndex, string orderBy, Order order, int[] expectedResultIds)
        {
            var retrievedLocalisations = await _localisationDao.SearchLocalisationsAsync(pageSize, pageIndex, null, orderBy, order);

            CollectionAssert.AreEqual(expectedResultIds, retrievedLocalisations.Select(localisation => localisation.LocalisationId));
        }

        [TestCase("", new[] { 1, 2}, TestName = "Empty filter should be interpreted by CountLocalisationsFoundAsync as no filter")]
        [TestCase(null, 8, TestName = "Null filter should be interpreted by CountLocalisationsFoundAsync as no filter")]
        [TestCase(" ", 8, TestName = "White space filter should be interpreted by CountLocalisationsFoundAsync as no filter")]
        [TestCase("AREA", 4, TestName = "CountLocalisationsFoundAsync should number of localisations that name contains AREA")]
        public async Task SearchLocalisationsAsync_should_apply_the_given_filter(string filter, int[] expectedResultIds)
        {
            var retrievedLocalisations = await _localisationDao.SearchLocalisationsAsync(4, 1, filter, nameof(Localisation.Name), Order.Asc);

            CollectionAssert.AreEquivalent(expectedResultIds, retrievedLocalisations.Select(localisation => localisation.LocalisationId));
        }

        [TestCase("", 8, TestName = "Empty filter should be interpreted by CountLocalisationsFoundAsync as no filter")]
        [TestCase(null, 8, TestName = "Null filter should be interpreted by CountLocalisationsFoundAsync as no filter")]
        [TestCase(" ", 8, TestName = "White space filter should be interpreted by CountLocalisationsFoundAsync as no filter")]
        [TestCase("AREA", 4, TestName = "CountLocalisationsFoundAsync should number of localisations that name contains AREA")]
        public async Task CountLocalisationsFoundAsync_should_apply_the_given_filter(string filter, int expectedCountResult)
        {
            var retrievedOfferCount = await _localisationDao.CountLocalisationsFoundAsync(filter);

            Assert.AreEqual(expectedCountResult, retrievedOfferCount);
        }
    }
}
