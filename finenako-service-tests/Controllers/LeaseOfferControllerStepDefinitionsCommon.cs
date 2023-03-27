
using System;
using System.Net;
using System.Net.Http;
using fonenako.DatabaseContexts;
using fonenako.Models;
using fonenako_service;
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

        [Given(@"The following list of lease offer is present in the system")]
        public void GivenTheFollowingListOfLeaseOfferIsPresentInTheSystem(Table table)
        {
            var leasOffers = table.CreateSet<LeaseOffer>();
            using var scope = _applicationFactory.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<FonenakoDbContext>();

            dbContext.RemoveRange(dbContext.LeaseOffers);
            dbContext.AddRange(leasOffers);
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
    }
}
