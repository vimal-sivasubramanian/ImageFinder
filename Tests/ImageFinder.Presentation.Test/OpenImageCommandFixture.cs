using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Presentation.Commands;
using Moq;
using NUnit.Framework;

namespace ImageFinder.Presentation.Test
{
    [TestFixture]
    class OpenImageCommandFixture
    {
        private OpenImageCommand _fixture;
        private ImageMetadata _imageMetadata;
        private Mock<IEventAggregator> _eventAggregatorMock;

        [SetUp]
        public void Setup()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _fixture = new OpenImageCommand(_eventAggregatorMock.Object);
            _imageMetadata = new ImageMetadata { Url = "https:\\random.com\random.jpg" };
        }

        [Test]
        [TestCase(null)]
        public void Invalid_Parameter_Value_To_CanExecute(object value)
        {
            Assert.IsFalse(_fixture.CanExecute(value));
        }

        [Test]
        [TestCase(null)]
        public void Invalid_Parameter_Value_To_Execute(object value)
        {
            _fixture.Execute(value);
            _eventAggregatorMock.Verify(_ => _.Publish(It.IsAny<ImageMetadata>()), Times.Never);
        }

        [Test]
        public void Valid_Parameter_Should_Be_Published()
        {
            _fixture.Execute(_imageMetadata);
            _eventAggregatorMock.Verify(_ => _.Publish(_imageMetadata), Times.Once);
        }
    }
}
