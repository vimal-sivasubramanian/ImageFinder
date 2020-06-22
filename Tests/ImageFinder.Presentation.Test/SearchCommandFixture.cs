using ImageFinder.CrossCutting.Exceptions;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Presentation.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageFinder.Presentation.Test
{
    [TestFixture]
    public class SearchCommandFixture
    {
        private SearchCommand _fixture;
        private Mock<IImageQueryService> _queryServiceMock;
        private Mock<IEventAggregator> _eventAggregatorMock;

        [SetUp]
        public void Setup()
        {
            _queryServiceMock = new Mock<IImageQueryService>();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger<SearchCommand>>();
            _fixture = new SearchCommand(_eventAggregatorMock.Object, loggerMock.Object, _queryServiceMock.Object);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Invalid_Parameter_Value_To_CanExecute(object value)
        {
            Assert.IsFalse(_fixture.CanExecute(value));
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Invalid_Parameter_Value_To_Execute(object value)
        {
            _fixture.Execute(value);
            _queryServiceMock.Verify(_ => _.QueryAsync(string.Empty), Times.Never);
        }

        [Test]
        public void Should_Trigger_Event_About_LongRunning_Process()
        {
            _fixture.Execute("check");
            _eventAggregatorMock.Verify(_ => _.Publish(It.IsAny<ProcessingState>()), Times.Exactly(2));
        }

        [Test]
        [TestCase("check")]
        [TestCase("check,nature")]
        public void Should_Trigger_QueryService_For_Valid_Criteria(string value)
        {
            _fixture.Execute(value);
            _queryServiceMock.Verify(_ => _.QueryAsync(value), Times.Once);
        }

        [Test]
        [TestCase("check")]
        [TestCase("check,nature")]
        public void QueryService_Result_Should_Be_Published(string value)
        {
            IList<ImageMetadata> images = Enumerable.Repeat(new ImageMetadata { Url = "https:\\random.com\random.jpg" }, 20).ToList();
            _queryServiceMock.Setup(_ => _.QueryAsync(value)).Returns(Task.FromResult(images));
            _fixture.Execute(value);
            _eventAggregatorMock.Verify(_ => _.Publish(images), Times.Once);
        }

        [Test]
        public void Should_Publish_Failure_On_Exception_Occurs()
        {
            _queryServiceMock.Setup(_ => _.QueryAsync("check")).Throws<ImageQueryException>();

            _fixture.Execute("check");

            _eventAggregatorMock.Verify(_ => _.Publish(It.IsAny<Error>()));

        }
    }
}