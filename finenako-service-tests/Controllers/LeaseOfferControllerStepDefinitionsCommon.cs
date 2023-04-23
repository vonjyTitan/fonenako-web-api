
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service;
using fonenako_service.Dtos;
using fonenako_service.Models;
using fonenako_service_tests.Controllers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace finenako_service_tests.Controllers
{
    [Binding]
    public class LeaseOfferControllerStepDefinitionsCommon
    {
        private readonly SpecFlowWebApplicationFactory<Startup> _applicationFactory;

        private readonly ScenarioContext _scenarioContext;

        public LeaseOfferControllerStepDefinitionsCommon(SpecFlowWebApplicationFactory<Startup> webApplicationFactory, ScenarioContext scenarioContext)
        {
            _applicationFactory = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [BeforeScenario]
        public void BeforeAnyScenario()
        {
            var scope = _applicationFactory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<FonenakoDbContext>();
            dbContext.Database.EnsureCreated();

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

            var lcs = dbContext.Localisations.ToArray();
            foreach (var leaseOffer in leasOffers)
            {
                var localisation = leaseOffer.Localisation;
                leaseOffer.Localisation = null;
                dbContext.Add(leaseOffer);
                if(localisation != null)
                {
                    dbContext.Localisations.First(loc => loc.LocalisationId == localisation.LocalisationId).LeaseOffers.Add(leaseOffer);
                }
            }

            dbContext.SaveChanges();
        }

        private LeaseOffer LeaseOfferParser(TableRow row)
        {
            var leaseOffer = row.CreateInstance<LeaseOffer>();
            leaseOffer.Localisation = new Localisation { LocalisationId = int.Parse(row["LocalisationId"]) };

            return leaseOffer;
        }

        [Given(@"The following list of localisations is present in the system")]
        public void GivenTheFollowingListOfLocalisationsIsPresentInTheSystem(Table table)
        {
            var localisations = table.CreateSet(LocalisationParser);
            var dbContext = _scenarioContext.Get<FonenakoDbContext>();

            foreach (var localisation in localisations)
            {
                if (localisation.Hierarchy != null)
                {
                    dbContext.Attach(localisation.Hierarchy);
                }
                dbContext.Add(localisation);
                dbContext.SaveChanges();
                dbContext.Entry(localisation).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                if(localisation.Hierarchy != null)
                {
                    dbContext.Entry(localisation.Hierarchy).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
            }


            var lcs = dbContext.Localisations.ToArray();
        }

        private Localisation LocalisationParser(TableRow row)
        {
            var response = row.CreateInstance<Localisation>();
            if (int.TryParse(row["HierarchyId"], out var hierarchyId))
            {
                response.Hierarchy = new Localisation { LocalisationId = hierarchyId };
            }
            return response;
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
                    Hierarchy = row.TryGetValue("Loc.Hier.Id", out var hierIdString) && int.TryParse(hierIdString, out var hierId) ? new LocalisationDto
                    {
                        LocalisationId = hierId,
                        Name = row.GetString("Loc.Hier.Name"),
                        Type = Enum.Parse<LocalisationType>(row.GetString("Loc.Hier.Type"))
                    } : null
                };
            }

            return dto;
        }
    }
}
