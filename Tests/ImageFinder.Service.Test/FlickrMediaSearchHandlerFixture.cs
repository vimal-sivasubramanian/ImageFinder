using ImageFinder.CrossCutting.Models;
using ImageFinder.Service.Handlers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
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

        [Test]
        public async Task When_Flickr_Responds_With_Valid_Feed()
        {
            string feed = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" +
                "<feed xmlns=\"http://www.w3.org/2005/Atom\">" +
                "<entry> <title>Call of summer</title>" +
                "<content type=\"html\">			&lt;p&gt;&lt;a href=&quot;https://www.flickr.com/people/184414591@N04/&quot;&gt;orlanefrye&lt;/a&gt; posted a photo:&lt;/p&gt;&lt;p&gt;&lt;a href=&quot;https://www.flickr.com/photos/184414591@N04/50036636753/&quot; title=&quot;Call of summer&quot;&gt;&lt;img src=&quot;https://live.staticflickr.com/65535/50036636753_5c55c5db8a_m.jpg&quot; width=&quot;240&quot; height=&quot;181&quot; alt=&quot;Call of summer&quot; /&gt;&lt;/a&gt;&lt;/p&gt;</content>" +
                "<author><name>orlanefrye</name></author>" +
                "<link rel=\"enclosure\" type=\"image/jpeg\" href=\"https://live.staticflickr.com/65535/50036636753_5c55c5db8a_b.jpg\" /> " +
                "</entry></feed>";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(feed),
            };
            MockHttpClientResponseWith(response);

            var result = await _fixture.Handle(new Queries.ImageSearchQuery(), default);

            Assert.IsTrue(result.Count == 1);

            Assert.AreEqual(result[0].ThumbnailUrl, "https://live.staticflickr.com/65535/50036636753_5c55c5db8a_m.jpg");
        }

        [Test]
        public async Task When_Flickr_Responds_With_Valid_Feed_With_HtmlEntities()
        {
            string feed = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" +
                "<feed xmlns=\"http://www.w3.org/2005/Atom\">" +
                "<entry> <title>Call of summer</title>" +
                "<content type=\"html\">			&lt;p&gt;&lt;a href=&quot;https://www.flickr.com/people/184414591@N04/&quot;&gt;orlanefrye&lt;/a&gt; posted a photo:&lt;/p&gt;&lt;p&gt;&lt;a href=&quot;https://www.flickr.com/photos/184414591@N04/50036636753/&quot; title=&quot;Call of&amp;nbsp;summer&quot;&gt;&lt;img src=&quot;https://live.staticflickr.com/65535/50036636753_5c55c5db8a_m.jpg&quot; width=&quot;240&quot; height=&quot;181&quot; alt=&quot;Call of summer&quot; /&gt;&lt;/a&gt;&lt;/p&gt;</content>" +
                "<author><name>orlanefrye</name></author>" +
                "<link rel=\"enclosure\" type=\"image/jpeg\" href=\"https://live.staticflickr.com/65535/50036636753_5c55c5db8a_b.jpg\" /> " +
                "</entry></feed>";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(feed),
            };
            MockHttpClientResponseWith(response);

            var result = await _fixture.Handle(new Queries.ImageSearchQuery(), default);

            Assert.IsTrue(result.Count == 1);

            Assert.AreEqual(result[0].ThumbnailUrl, "https://live.staticflickr.com/65535/50036636753_5c55c5db8a_m.jpg");
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
