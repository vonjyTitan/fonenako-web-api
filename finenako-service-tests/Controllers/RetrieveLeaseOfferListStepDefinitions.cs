using System;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using finenako_service_tests.Controllers;
using fonenako_service;
using fonenako_service.Dtos;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace fonenako_service_tests.Controllers
{
    [Binding]
    public class RetrieveLeaseOfferListStepDefinitions
    {
        private const string Uri = "api/V1/Lease-offers";

        private readonly SpecFlowWebApplicationFactory<Startup> _applicationFactory;

        private readonly ScenarioContext _scenarioContext;

        private Pageable<LeaseOfferDto> _responseBody;

        public RetrieveLeaseOfferListStepDefinitions(SpecFlowWebApplicationFactory<Startup> webApplicationFactory, ScenarioContext scenarioContext)
        {
            _applicationFactory = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }

        private void SendRequest(string uri)
        {
            _scenarioContext.Set(_applicationFactory.CreateDefaultClient().GetAsync(uri).Result);
        }

        private void ParseBodyIfNeed()
        {
            if (_responseBody != null) return;

            var body = _scenarioContext.Get<HttpResponseMessage>().Content.ReadAsStringAsync().Result;
            _responseBody = JsonSerializer.Deserialize<Pageable<LeaseOfferDto>>(body);
        }

        [When(@"I make a GET request on lease-offers endpoint with arguments '(.*)'")]
        public void WhenIMakeAGETRequestOnLease_OffersEndpointWithArguments(string param)
        {
            SendRequest($"{Uri}?{param}");
        }

        [When(@"I make a GET request on lease-offers endpoint with pagination : '(.*)' and filter : '(.*)'")]
        public void WhenIMakeAGETRequestOnLease_OffersEndpointWithPaginationAndFilter(string pagination, string filter)
        {
            SendRequest($"{Uri}?{pagination}&filter={HttpUtility.UrlEncode(filter)}");
        }

        [When(@"I make a GET request on lease-offers endpoint with filter : '(.*)'")]
        public void WhenIMakeAGETRequestOnLease_OffersEndpointWithFilter(string filter)
        {
            SendRequest($"{Uri}?filter={HttpUtility.UrlEncode(filter)}");
        }

        [When(@"I make a GET request on lease-offers endpoint without any argument")]
        public void WhenIMakeAGETRequestOnLease_OffersEndpointWithoutAnyArgument()
        {
            SendRequest(Uri);
        }

        [Then(@"The pageable infos should be like : \{CurrentPage : '(.*)', TotalPage : '(.*)', PageSize : '(.*)', totalFound : '(.*)'}")]
        public void ThenThePageableInfosShouldBeLikeCurrentPageTotalPagePageSizeTotal(int expectedCurrentpage, int expectedTotalPage, int expectedPageSize, int expectedTotalFound)
        {
            ParseBodyIfNeed();

            Assert.AreEqual(expectedCurrentpage, _responseBody.CurrentPage);
            Assert.AreEqual(expectedPageSize, _responseBody.PageSize);
            Assert.AreEqual(expectedTotalPage, _responseBody.TotalPage);
            Assert.AreEqual(expectedTotalFound, _responseBody.TotalFound);
        }

        [Then(@"The pageable content items should be like :")]
        public void ThenTheResultBodyShouldBeLikeThis(Table table)
        {
            ParseBodyIfNeed();
            var expectedContent = table.CreateSet(LeaseOfferControllerStepDefinitionsCommon.LeaseOfferDtoParser);

            Assert.AreEqual(JsonSerializer.Serialize(expectedContent), JsonSerializer.Serialize(_responseBody.Content), "The lease offer list in the response body as Json is not equal to the expectation");
        }
    }
}