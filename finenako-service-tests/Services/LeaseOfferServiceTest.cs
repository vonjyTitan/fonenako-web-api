
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fonenako.Models;
using fonenako_service;
using fonenako_service.Daos;
using fonenako_service.Dtos;
using fonenako_service.Services;
using Moq;
using NUnit.Framework;

namespace fonenako_service_tests.Services
{
    [TestFixture]
    public class LeaseOfferServiceTest
    {
        private LeaseOfferService _leaseOfferService;

        private Mock<ILeaseOfferDao> _leaseOfferDaoMock;

        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _leaseOfferDaoMock = new Mock<ILeaseOfferDao>();
            _mapperMock = new Mock<IMapper>();
            _leaseOfferService = new LeaseOfferService(_leaseOfferDaoMock.Object, _mapperMock.Object);
        }

        [TestCase(0, 1, nameof(LeaseOfferDto.LeaseOfferID), TestName = "Page size 0 should make the method rise ArgumentException")]
        [TestCase(10, 0, nameof(LeaseOfferDto.LeaseOfferID), TestName = "Page index 0 should make the method rise ArgumentException")]
        [TestCase(10, 1, nameof(LeaseOfferDto.CarouselUri), TestName = "Field not orderable should make the method rise ArgumentException")]
        [TestCase(10, 1, "UnknownField", TestName = "Unknown Field should make the method rise ArgumentException")]
        public void RetrieveLeaseOffersAsync_should_throw_argumentexcption_when_called_with_wrong_arg(int pageSize, int pageIndex, string orderBy)
        {
             Assert.ThrowsAsync<ArgumentException>(()=> _leaseOfferService.RetrieveLeaseOffersAsync(pageSize, pageIndex, LeaseOfferFilter.Default, orderBy, Order.Asc));
        }

        [TestCase(1, 1, 15, 3, TestName = "The total page should be 15/5 = 3")]
        [TestCase(2, 1, 5, 1, TestName = "The current page index should be reduced from 2 to 1 when the requested page index is out of range")]
        [TestCase(1, 1, 16, 4, TestName = "The total page should take the next int value when there is an element after the multipl of page size")]
        public async Task RetrieveLeaseOffersAsync_should_return_a_correct_pageable(int pageIndex, int expectedPageIndexResult, int totalItems, int expectedTotalPage)
        {
            const int pageSize = 5;
            const string orderBy = nameof(LeaseOffer.LeaseOfferID);
            const Order order = Order.Asc;

            var leaseOfferModels = new []{ new LeaseOffer()};
            var leaseOfferDtos = new[] { new LeaseOfferDto() };
            _leaseOfferDaoMock.Setup(dao => dao.CountLeaseOffersAsync(LeaseOfferFilter.Default)).ReturnsAsync(totalItems);
            _leaseOfferDaoMock.Setup(dao => dao.RetrieveLeaseOffersByPageAsync(pageSize, expectedPageIndexResult, LeaseOfferFilter.Default, orderBy, order)).ReturnsAsync(leaseOfferModels);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<LeaseOfferDto>>(leaseOfferModels)).Returns(leaseOfferDtos);

            var pageableResult = await _leaseOfferService.RetrieveLeaseOffersAsync(pageSize, pageIndex, LeaseOfferFilter.Default, orderBy, order);

            Assert.IsNotNull(pageableResult);
            Assert.AreEqual(expectedPageIndexResult, pageableResult.CurrentPage);
            Assert.AreEqual(pageSize, pageableResult.PageSize);
            Assert.AreEqual(expectedTotalPage, pageableResult.TotalPage);
            CollectionAssert.AreEqual(leaseOfferDtos, pageableResult.Content);
        }

        [Test]
        public async Task RetrieveLeaseOffersAsync_should_return_empty_when_there_is_no_retrieved_item()
        {
            _leaseOfferDaoMock.Setup(dao => dao.CountLeaseOffersAsync(LeaseOfferFilter.Default)).ReturnsAsync(0);

            var pageableResult = await _leaseOfferService.RetrieveLeaseOffersAsync(2, 2, LeaseOfferFilter.Default, nameof(LeaseOffer.LeaseOfferID), Order.Asc);

            Assert.IsNotNull(pageableResult);
            Assert.AreEqual(1, pageableResult.CurrentPage);
            Assert.AreEqual(2, pageableResult.PageSize);
            Assert.AreEqual(0, pageableResult.TotalPage);
            CollectionAssert.IsEmpty(pageableResult.Content);
        }

        [Test]
        public async Task FindLeaseOfferByIdAsync_should_return_null_when_dao_returns_null()
        {
            var leaseOffer = await _leaseOfferService.FindLeaseOfferByIdAsync(1);

            Assert.IsNull(leaseOffer);
        }

        [Test]
        public async Task FindLeaseOfferByIdAsync_should_return_the_right_dto_when_dao_and_mapper_return_the_right_object()
        {
            const int leaseOfferId = 1;
            var leaseOffer = new LeaseOffer
            {
                LeaseOfferID = leaseOfferId
            };
            var expectedLeaseOfferDto = new LeaseOfferDto
            {
                LeaseOfferID = leaseOfferId
            };

            _leaseOfferDaoMock.Setup(dao => dao.FindLeaseOfferDetailsByIdAsync(leaseOfferId)).ReturnsAsync(leaseOffer);
            _mapperMock.Setup(mapper => mapper.Map<LeaseOfferDto>(leaseOffer)).Returns(expectedLeaseOfferDto);

            var leaseOfferResult = await _leaseOfferService.FindLeaseOfferByIdAsync(1);

            Assert.AreEqual(expectedLeaseOfferDto, leaseOfferResult);
        }
    }
}
