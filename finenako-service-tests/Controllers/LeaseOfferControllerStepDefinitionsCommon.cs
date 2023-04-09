
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service;
using fonenako_service.Dtos;
using fonenako_service.Models;
using fonenako_service_tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace finenako_service_tests.Controllers
{
    [Binding]
    public class LeaseOfferControllerStepDefinitionsCommon
    {
        private readonly SpecFlowWebApplicationFactory<Program> _applicationFactory;

        private readonly ScenarioContext _scenarioContext;

        public LeaseOfferControllerStepDefinitionsCommon(SpecFlowWebApplicationFactory<Program> webApplicationFactory, ScenarioContext scenarioContext)
        {
            _applicationFactory = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [BeforeScenario]
        public void BeforeAnyScenario()
        {
            var scope = _applicationFactory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<FonenakoDbContext>();

            //Cleanup DB
            dbContext.RemoveRange(dbContext.LeaseOffers);
            dbContext.RemoveRange(dbContext.Cities);
            dbContext.RemoveRange(dbContext.Areas);
            dbContext.SaveChanges();

            _scenarioContext.Set(scope);
            _scenarioContext.Set(dbContext);
        }

        [AfterScenario]
        public void AfterAnyScenario()
        {
            _scenarioContext.Get<FonenakoDbContext>()?.Dispose();
            _scenarioContext.Get<IServiceScope>()?.Dispose();
        }

        [Given(@"The following list of lease offer is present in the system")]
        public void GivenTheFollowingListOfLeaseOfferIsPresentInTheSystem(Table table)
        {
            var leasOffers = table.CreateSet(LeaseOfferParser);
            var dbContext = _scenarioContext.Get<FonenakoDbContext>();

            foreach(var leaseOffer in leasOffers)
            {
                dbContext.Add(leaseOffer);
                dbContext.Areas.Include(area => area.LeaseOffers).First(a => a.AreaId == leaseOffer.AreaId).LeaseOffers.Add(leaseOffer);
            }

            dbContext.SaveChanges();
            var leaseOffers = dbContext.LeaseOffers.AsNoTracking().Include(leaseOffer => leaseOffer.Area).ToArray();
        }

        private LeaseOffer LeaseOfferParser(TableRow row)
        {
            var leaseOffer = row.CreateInstance<LeaseOffer>();
            leaseOffer.AreaId = int.Parse(row["AreaId"]);

            return leaseOffer;
        }

        [Given(@"The following list of city is present in the system")]
        public void GivenTheFollowingListOfCityIsPresentInTheSystem(Table table)
        {
            var cities = table.CreateSet<City>();
            var dbContext = _scenarioContext.Get<FonenakoDbContext>();

            dbContext.AddRange(cities);
            dbContext.SaveChanges();
        }

        [Given(@"The following list of area is present in the system")]
        public void GivenTheFollowingListOfAreaIsPresentInTheSystem(Table table)
        {
            var areas = table.CreateSet<Area>();
            var dbContext = _scenarioContext.Get<FonenakoDbContext>();

            foreach(var area in areas)
            {
                dbContext.Add(area);
                dbContext.Cities.Include(City => City.Areas).FirstOrDefault(city => city.CityId == area.CityId).Areas.Add(area);
            }

            dbContext.SaveChanges();

        }

        [Given(@"Whatever data I have in the system")]
        public void GivenWhateverDataIHaveInTheSystem()
        {
        }

        [Then(@"The response Status code should be '(.*)'")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var httpResponse = _scenarioContext.Get<HttpResponseMessage>();

            Assert.AreEqual((HttpStatusCode)expectedStatusCode, httpResponse.StatusCode, $"Actual body : {httpResponse.Content.ReadAsStringAsync().Result}");
        }

        public static LeaseOfferDto LeaseOfferDtoParser(TableRow row)
        {
            var dto = row.CreateInstance<LeaseOfferDto>();
            dto.PhotoUris = row.GetString("PhotoUris").Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (row.ContainsKey("Area.Id"))
            {
                dto.Area = new AreaDto
                {
                    AreaId = row.GetInt32("Area.Id"),
                    Name = row.GetString("Area.Name"),
                    City = new CityDto
                    {
                        CityId = row.GetInt32("Area.City.Id"),
                        Name = row.GetString("Area.City.Name")
                    }
                };
            }

            return dto;
        }
    }
}
