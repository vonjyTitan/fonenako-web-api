using System;
using System.Net.Http;
using System.Text.Json;
using fonenako_service;
using fonenako_service.Dtos;
using fonenako_service.Models;
using fonenako_service_tests.Controllers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace finenako_service_tests.Controllers
{
    [Binding]
    public class RetrieveLocalisationsListSetpDefinitions
    {
        private const string Uri = "api/V1/Localisations";

        private readonly ScenarioContext _scenarioContext;

        private readonly SpecFlowWebApplicationFactory<Startup> _applicationFactory;

        private Pageable<LocalisationDto> _responseBody;

        public RetrieveLocalisationsListSetpDefinitions(ScenarioContext scenarioContext, SpecFlowWebApplicationFactory<Startup> applicationFactory)
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
            if (_responseBody != null) return;

            var body = _scenarioContext.Get<HttpResponseMessage>().Content.ReadAsStringAsync().Result;
            _responseBody = JsonSerializer.Deserialize<Pageable<LocalisationDto>>(body);
        }

        [When(@"I make a GET request on localisations api with parameters : '(.*)'")]
        public void WhenIMakeAGETRequestOnLocalisationsApiWithParameters(string param)
        {
            SendRequest($"{Uri}?{param}");
        }

        [Then(@"The pageable localisation infos should be like : \{CurrentPage : '(.*)', TotalPage : '(.*)', PageSize : '(.*)', totalFound : '(.*)'}")]
        public void ThenThePageableLocalisationInfosShouldBeLikeCurrentPageTotalPagePageSizeTotalFound(int expectedCurrentpage, int expectedTotalPage, int expectedPageSize, int expectedTotalFound)
        {
            ParseBodyIfNeed();

            Assert.AreEqual(expectedCurrentpage, _responseBody.CurrentPage);
            Assert.AreEqual(expectedPageSize, _responseBody.PageSize);
            Assert.AreEqual(expectedTotalPage, _responseBody.TotalPage);
            Assert.AreEqual(expectedTotalFound, _responseBody.TotalFound);
        }

        [Then(@"The response content as localisations list should be like :")]
        public void ThenTheResponseContentAsLocalisationsListShouldBeLike(Table table)
        {
            ParseBodyIfNeed();
            var expectedContent = table.CreateSet(CreateLocalisationInstance);

            Assert.AreEqual(JsonSerializer.Serialize(expectedContent), JsonSerializer.Serialize(_responseBody.Content), "The localisations list in the response body as Json is not equal to the expectation");
        }

        private LocalisationDto CreateLocalisationInstance(TableRow row)
        {
            var dto = row.CreateInstance<LocalisationDto>();
            if(row.TryGetValue("Hier.Id", out var hierIdString) && int.TryParse(hierIdString, out var hierId))
            {
                dto.Hierarchy = new LocalisationDto
                {
                    LocalisationId = hierId,
                    Name = row.GetString("Hier.Name"),
                    Type = Enum.Parse<LocalisationType>(row.GetString("Hier.Type"))
                };
            }
            return dto;
        }
    }
}
