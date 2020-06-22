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
    [TestFixture]
    public class TweetQueryServiceFixture
    {
        private Mock<IMediator> _mediatrMock;
        ITweetQueryService _fixture;

        [SetUp]
        public void Setup()
        {
            var loggerMock = new Mock<ILogger<TweetQueryService>>();
            _mediatrMock = new Mock<IMediator>();
            _fixture = new TweetQueryService(_mediatrMock.Object, loggerMock.Object);
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
            _mediatrMock.Verify(_ => _.Send(It.Is<TweetSearchQuery>(query => query.Criteria == value), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Should_Handle_Exception_Thrown_By_Handlers()
        {
            _mediatrMock.Setup(_ => _.Send(It.IsAny<TweetSearchQuery>(), It.IsAny<CancellationToken>())).Throws<HttpRequestException>();

            Assert.ThrowsAsync<TweetQueryException>(() => _fixture.QueryAsync("nature"));

            _mediatrMock.Setup(_ => _.Send(It.IsAny<TweetSearchQuery>(), It.IsAny<CancellationToken>())).Throws<Exception>();

            Assert.ThrowsAsync<TweetQueryException>(() => _fixture.QueryAsync("nature"));
        }

        [Test]
        public void Should_Return_Result_From_QueryHandlers()
        {
            IList<Tweet> images = Enumerable.Repeat(new Tweet { Message = "Random", Author = "Vimal" }, 20).ToList();

            _mediatrMock.Setup(_ => _.Send(It.IsAny<TweetSearchQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(images));

            Assert.AreEqual(_fixture.QueryAsync("nature").Result, images);
        }
    }
}