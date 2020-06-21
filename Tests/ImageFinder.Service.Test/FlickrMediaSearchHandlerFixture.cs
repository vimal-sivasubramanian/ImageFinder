using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Service.Handlers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ImageFinder.Service.Test
{
    [TestFixture]
    public class FlickrMediaSearchHandlerFixture
    {
        private FlickrMediaSearchHandler _fixture;
        private Mock<LoggerMock> _loggerMock;
        private Mock<HttpMessageHandler> _handlerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<LoggerMock>();
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object);
            _fixture = new FlickrMediaSearchHandler(httpClient, _loggerMock.Object);
        }

        [Test]
        public void Handle_Invalid_Request()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Handle(null, default));
        }

        [Test]
        public void Should_Throw_Exception_When_Flickr_Response_StatusCode_Not_200()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            MockHttpClientResponseWith(response);

            Assert.ThrowsAsync<HttpRequestException>(() => _fixture.Handle(new Queries.ImageSearchQuery(), default));
        }

        [Test]
        public void When_Flickr_Responds_No_Images()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty),
            };
            MockHttpClientResponseWith(response);

            Assert.DoesNotThrowAsync(() => _fixture.Handle(new Queries.ImageSearchQuery(), default));

            Assert.AreEqual( _fixture.Handle(new Queries.ImageSearchQuery(), default).Result, It.IsAny<IList<ImageMetadata>>());
        }

        private void MockHttpClientResponseWith(HttpResponseMessage response)
        {
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        public class LoggerMock : ILogger<FlickrMediaSearchHandler>
        {
            public IDisposable BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => false;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            { }
        }
    }
}
