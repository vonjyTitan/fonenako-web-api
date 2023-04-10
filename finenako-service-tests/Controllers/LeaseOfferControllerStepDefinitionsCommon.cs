
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
            dbContext.RemoveRange(dbContext.Localisations);
            dbContext.RemoveRange(dbContext.LeaseOffers);
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
                dbContext.Localisations.First(a => a.LocalisationId == leaseOffer.LocalisationId).LeaseOffers.Add(leaseOffer);
            }

            dbContext.SaveChanges();
        }

        private LeaseOffer LeaseOfferParser(TableRow row)
        {
            var leaseOffer = row.CreateInstance<LeaseOffer>();
            leaseOffer.LocalisationId = int.Parse(row["LocalisationId"]);

            return leaseOffer;
        }

        [Given(@"The following list of localisations is present in the system")]
        public void GivenTheFollowingListOfLocalisationsIsPresentInTheSystem(Table table)
        {
            var localisations = table.CreateSet<Localisation>();
            var dbContext = _scenarioContext.Get<FonenakoDbContext>();

            foreach (var localisation in localisations)
            {
                dbContext.Add(localisation);
                if(localisation.HierarchyId.HasValue)
                {
                    localisations.FirstOrDefault(hier => hier.LocalisationId == localisation.HierarchyId).SubLocalisations
                                           .Add(localisation);
                }
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

            if (row.ContainsKey("Loc.Id"))
            {
                dto.Localisation = new LocalisationDto
                {
                    LocalisationId = row.GetInt32("Loc.Id"),
                    Name = row.GetString("Loc.Name"),
                    Type = Enum.Parse<LocalisationType>(row.GetString("Loc.Type")),
                    Hierarchy = row.TryGetValue("Loc.Hier.Id", out var hierId) ? new LocalisationDto
                    {
                        LocalisationId = int.Parse(hierId),
                        Name = row.GetString("Loc.Hier.Name"),
                        Type = Enum.Parse<LocalisationType>(row.GetString("Loc.Hier.Type"))
                    } : null
                };
            }

            return dto;
        }
    }
}
