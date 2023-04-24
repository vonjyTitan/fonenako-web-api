using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Models;
using fonenako_service.Services;
using Moq;
using NUnit.Framework;

namespace finenako_service_tests.Services
{
    [TestFixture]
    public class LocalisationServiceTest
    {
        private LocalisationService _localisationService;

        private Mock<ILocalisationDao> _localisationDaoMock;

        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _localisationDaoMock = new Mock<ILocalisationDao>();
            _mapperMock = new Mock<IMapper>();
            _localisationService = new LocalisationService(_localisationDaoMock.Object, _mapperMock.Object);
        }

        [TestCase(0, 1, nameof(LocalisationDto.Name), TestName = "Page size 0 should make SearchLocalisationsAsync rise ArgumentException")]
        [TestCase(10, 0, nameof(LocalisationDto.Name), TestName = "Page index 0 should make SearchLocalisationsAsync rise ArgumentException")]
        [TestCase(10, 1, nameof(LocalisationDto.LocalisationId), TestName = "Field not sortable should make SearchLocalisationsAsync rise ArgumentException")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown Field should make SearchLocalisationsAsync rise ArgumentException")]
        public void SearchLocalisationsAsync_should_throw_argumentexcption_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _localisationService.SearchLocalisationsAsync(pageSize, pageIndex, null, orderBy, Order.Asc));
        }

        [TestCase(1, 1, 15, 3, TestName = "The total page returned by SearchLocalisationsAsync should be 15/5 = 3")]
        [TestCase(2, 1, 5, 1, TestName = "The current page index returned by SearchLocalisationsAsync should be reduced from 2 to 1 when the requested page index is out of range")]
        [TestCase(1, 1, 16, 4, TestName = "The total page returned by SearchLocalisationsAsync should take the next int value when there is an element after the multipl of page size")]
        public async Task SearchLocalisationsAsync_should_return_a_correct_pageable(int pageIndex, int expectedPageIndexResult, int totalItems, int expectedTotalPage)
        {
            const int pageSize = 5;
            const string orderBy = nameof(LocalisationDto.Name);
            const Order order = Order.Asc;

            var localisationModels = new[] { new Localisation() };
            var localisationDtos = new[] { new LocalisationDto() };
            _localisationDaoMock.Setup(dao => dao.CountLocalisationsFoundAsync("test")).ReturnsAsync(totalItems);
            _localisationDaoMock.Setup(dao => dao.SearchLocalisationsAsync(pageSize, expectedPageIndexResult, "test", orderBy, order)).ReturnsAsync(localisationModels);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<LocalisationDto>>(localisationModels)).Returns(localisationDtos);

            var pageableResult = await _localisationService.SearchLocalisationsAsync(pageSize, pageIndex, "test", orderBy, order);

            Assert.IsNotNull(pageableResult);
            Assert.AreEqual(expectedPageIndexResult, pageableResult.CurrentPage);
            Assert.AreEqual(pageSize, pageableResult.PageSize);
            Assert.AreEqual(expectedTotalPage, pageableResult.TotalPage);
            CollectionAssert.AreEqual(localisationDtos, pageableResult.Content);
        }

        [Test]
        public async Task SearchLocalisationsAsync_should_return_empty_when_there_is_no_retrieved_item()
        {
            _localisationDaoMock.Setup(dao => dao.CountLocalisationsFoundAsync("SpecialLocName")).ReturnsAsync(0);

            var pageableResult = await _localisationService.SearchLocalisationsAsync(2, 2, "SpecialLocName", nameof(LocalisationDto.Name), Order.Asc);

            Assert.IsNotNull(pageableResult);
            Assert.AreEqual(1, pageableResult.CurrentPage);
            Assert.AreEqual(2, pageableResult.PageSize);
            Assert.AreEqual(0, pageableResult.TotalPage);
            CollectionAssert.IsEmpty(pageableResult.Content);
        }
    }
}
