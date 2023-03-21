

using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using fonenako_service;
using fonenako_service.Dtos;
using fonenako_service_tests;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace finenako_service_tests.Controllers
{
    [Binding]
    public class RetrieveSingleLeaseOfferStepDefinitions
    {
        private readonly SpecFlowWebApplicationFactory<Program> _applicationFactory;

        private readonly ScenarioContext _scenarioContext;

        protected RetrieveSingleLeaseOfferStepDefinitions(SpecFlowWebApplicationFactory<Program> webApplicationFactory, ScenarioContext scenarioContext)
        {
            _applicationFactory = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        [When(@"I make a GET request on lease-offers endpoint with offer id : '(.*)'")]
        public void WhenIMakeAGETRequestOnLease_OffersEndpointWithOfferId(string leaseOfferId)
        {
            _scenarioContext.Set(_applicationFactory.CreateDefaultClient().GetAsync($"api/V1/Lease-offers/{leaseOfferId}").Result);
        }

        [Then(@"The body content should be like :")]
        public void ThenTheBodyContentShouldBeLike(Table table)
        {
            var body = _scenarioContext.Get<HttpResponseMessage>().Content.ReadAsStringAsync().Result;
            var leaseOffer = JsonSerializer.Deserialize<LeaseOfferDto>(body);

            Assert.AreEqual(JsonSerializer.Serialize(table.CreateSet<LeaseOfferDto>().FirstOrDefault()), JsonSerializer.Serialize(leaseOffer), "The response body as Json is not equal to the expectation");
        }
    }
}
