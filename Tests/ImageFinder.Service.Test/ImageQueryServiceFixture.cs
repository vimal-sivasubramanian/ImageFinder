using ImageFinder.CrossCutting.Exceptions;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Service.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ImageFinder.Service.Test
{
    public class ImageQueryServiceFixture
    {
        private Mock<IMediator> _mediatrMock;
        IImageQueryService _fixture;

        [SetUp]
        public void Setup()
        {
            var loggerMock = new Mock<ILogger<ImageQueryService>>();
            _mediatrMock = new Mock<IMediator>();
            _fixture = new ImageQueryService(loggerMock.Object, _mediatrMock.Object);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Should_Throw_Exception_For_Invalid_Criteria_Value(string value)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _fixture.QueryAsync(value));
        }

        [Test]
        [TestCase("nature")]
        [TestCase("check")]
        public void Should_Publish_Valid_Criteria_To_QueryHandlers(string value)
        {
            _fixture.QueryAsync(value);
            _mediatrMock.Verify(_ => _.Send(It.Is<ImageSearchQuery>(query => query.Criteria == value), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Should_Handle_Exception_Thrown_By_Handlers()
        {
            _mediatrMock.Setup(_ => _.Send(It.IsAny<ImageSearchQuery>(), It.IsAny<CancellationToken>())).Throws<HttpRequestException>();

            Assert.ThrowsAsync<ImageQueryException>(() => _fixture.QueryAsync("nature"));

            _mediatrMock.Setup(_ => _.Send(It.IsAny<ImageSearchQuery>(), It.IsAny<CancellationToken>())).Throws<Exception>();

            Assert.ThrowsAsync<ImageQueryException>(() => _fixture.QueryAsync("nature"));
        }

        [Test]
        public void Should_Return_Result_From_QueryHandlers()
        {
            IList<ImageMetadata> images = Enumerable.Repeat(new ImageMetadata { Url = "https:\\random.com\random.jpg" }, 20).ToList();

            _mediatrMock.Setup(_ => _.Send(It.IsAny<ImageSearchQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(images));

            Assert.AreEqual(_fixture.QueryAsync("nature").Result, images);
        }
    }
}