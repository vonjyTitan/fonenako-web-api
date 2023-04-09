
using System;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using fonenako_service;
using fonenako_service.Dtos;
using fonenako_service_tests;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace finenako_service_tests.Controllers
{
    [Binding]
    public class RetrieveCitiesStepDefinitions
    {
        private const string Uri = "api/V1/Cities";

        private readonly ScenarioContext _scenarioContext;

        private readonly SpecFlowWebApplicationFactory<Program> _applicationFactory;

        private Pageable<CityDto> _responseBody;

        public RetrieveCitiesStepDefinitions(ScenarioContext scenarioContext, SpecFlowWebApplicationFactory<Program> applicationFactory)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            _applicationFactory = applicationFactory ?? throw new ArgumentNullException(nameof(applicationFactory));
        }

        private void SendRequest(string uri)
        {
            _scenarioContext.Set(_applicationFactory.CreateDefaultClient().GetAsync(uri).Result);
        }

        private void ParseBodyIfNeed()
        {
            if(_responseBody == null)
            {
                var body = _scenarioContext.Get<HttpResponseMessage>().Content.ReadAsStringAsync().Result;
                _responseBody = JsonSerializer.Deserialize<Pageable<CityDto>>(body);
            }
        }

        [Given(@"The city and area default values are present in the system")]
        public void GivenTheCityAndAreaDefaultValuesArePresentInTheSystem()
        {
            //Do nothing
        }

        [When(@"I make a GET request on cities endpoint with pagination '(.*)'")]
        public void WhenIMakeAGETRequestOnCitiesEndpointWithPagination(string pagination)
        {
            SendRequest($"{Uri}?{pagination}");
        }

        [When(@"I make a GET request on cities endpoint without any argument")]
        public void WhenIMakeAGETRequestOnCitiesEndpointWithoutAnyArgument()
        {
            SendRequest(Uri);
        }

        [Then(@"The pageable city infos should be like : \{CurrentPage : '(.*)', TotalPage : '(.*)', PageSize : '(.*)', totalFound : '(.*)'}")]
        public void ThenThePageableCityInfosShouldBeLikeCurrentPageTotalPagePageSizeTotalFound(int expectedCurrentPage, int expectedTotalPage, int expectedPageSize, int expectedTotalFound)
        {
            ParseBodyIfNeed();

            Assert.AreEqual(expectedCurrentPage, _responseBody.CurrentPage);
            Assert.AreEqual(expectedPageSize, _responseBody.PageSize);
            Assert.AreEqual(expectedTotalPage, _responseBody.TotalPage);
            Assert.AreEqual(expectedTotalFound, _responseBody.TotalFound);
        }

        [Then(@"The pageable city content items should be like :")]
        public void ThenThePageableCityContentItemsShouldBeLike(Table table)
        {
            ParseBodyIfNeed();

            var expectedContent = table.CreateSet(ParseCityDto);
            Assert.AreEqual(JsonSerializer.Serialize(expectedContent), JsonSerializer.Serialize(_responseBody.Content), "The lease offer list in the response body as Json is not equal to the expectation");
        }

        private CityDto ParseCityDto(TableRow row)
        {
            var cityDto = row.CreateInstance<CityDto>();
            var parsedAreaIds = row["AreasId"].Split(',');
            var parsedAreaNames = row["AreasName"].Split(',');
            for(var i = 0; i< parsedAreaIds.Length; i++)
            {
                cityDto.Areas.Add(new()
                {
                    AreaId = int.Parse(parsedAreaIds[i]),
                    Name = parsedAreaNames[i]
                });
            }
            return cityDto;
        }

        [When(@"I make a GET request on cities endpoint with filter '(.*)'")]
        public void WhenIMakeAGETRequestOnCitiesEndpointWithFilter(string filter)
        {
            SendRequest($"{Uri}?filter={HttpUtility.UrlEncode(filter)}");
        }
    }
}
