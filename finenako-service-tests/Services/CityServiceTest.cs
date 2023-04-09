using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Models;
using fonenako_service.Services;
using Moq;
using NUnit.Framework;

namespace finenako_service_tests.Services
{
    [TestFixture]
    public class CityServiceTest
    {
        private CityService _cityService;

        private Mock<ICityDao> _cityDaoMock;

        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _cityDaoMock = new Mock<ICityDao>();
            _mapperMock = new Mock<IMapper>();
            _cityService = new CityService(_cityDaoMock.Object, _mapperMock.Object);
        }

        [TestCase(0, 1, nameof(CityDto.Name), TestName = "Page size 0 should make the method rise ArgumentException")]
        [TestCase(10, 0, nameof(CityDto.Name), TestName = "Page index 0 should make the method rise ArgumentException")]
        [TestCase(10, 1, nameof(CityDto.CityId), TestName = "Field not sortable should make the method rise ArgumentException")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown Field should make the method rise ArgumentException")]
        public void RetrieveCitiesAsync_should_throw_argumentexcption_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _cityService.RetrieveCitiesAsync(pageSize, pageIndex, CityFilter.Default, orderBy, Order.Asc));
        }

        [TestCase(1, 1, 15, 3, TestName = "The total page should be 15/5 = 3")]
        [TestCase(2, 1, 5, 1, TestName = "The current page index should be reduced from 2 to 1 when the requested page index is out of range")]
        [TestCase(1, 1, 16, 4, TestName = "The total page should take the next int value when there is an element after the multipl of page size")]
        public async Task RetrieveCitiesAsync_should_return_a_correct_pageable(int pageIndex, int expectedPageIndexResult, int totalItems, int expectedTotalPage)
        {
            const int pageSize = 5;
            const string orderBy = nameof(CityDto.Name);
            const Order order = Order.Asc;

            var cityModels = new[] { new City() };
            var cityDtos = new[] { new CityDto() };
            _cityDaoMock.Setup(dao => dao.CountCitiesAsync(CityFilter.Default)).ReturnsAsync(totalItems);
            _cityDaoMock.Setup(dao => dao.RetrieveCitiesByPageAsync(pageSize, expectedPageIndexResult, CityFilter.Default, orderBy, order)).ReturnsAsync(cityModels);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CityDto>>(cityModels)).Returns(cityDtos);

            var pageableResult = await _cityService.RetrieveCitiesAsync(pageSize, pageIndex, CityFilter.Default, orderBy, order);

            Assert.IsNotNull(pageableResult);
            Assert.AreEqual(expectedPageIndexResult, pageableResult.CurrentPage);
            Assert.AreEqual(pageSize, pageableResult.PageSize);
            Assert.AreEqual(expectedTotalPage, pageableResult.TotalPage);
            CollectionAssert.AreEqual(cityDtos, pageableResult.Content);
        }

        [Test]
        public async Task RetrieveCitiesAsync_should_return_empty_when_there_is_no_retrieved_item()
        {
            _cityDaoMock.Setup(dao => dao.CountCitiesAsync(CityFilter.Default)).ReturnsAsync(0);

            var pageableResult = await _cityService.RetrieveCitiesAsync(2, 2, CityFilter.Default, nameof(CityDto.Name), Order.Asc);

            Assert.IsNotNull(pageableResult);
            Assert.AreEqual(1, pageableResult.CurrentPage);
            Assert.AreEqual(2, pageableResult.PageSize);
            Assert.AreEqual(0, pageableResult.TotalPage);
            CollectionAssert.IsEmpty(pageableResult.Content);
        }
    }
}
