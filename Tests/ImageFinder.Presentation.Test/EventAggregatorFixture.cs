using ImageFinder.CrossCutting;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using Moq;
using NUnit.Framework;
using System;

namespace ImageFinder.Presentation.Test
{
    [TestFixture(typeof(Error))]
    [TestFixture(typeof(ImageMetadata))]
    public class EventAggregatorFixture<T> where T : class
    {
        private IEventAggregator _eventAggregator;

        [SetUp]
        public void Setup()
        {
            _eventAggregator = new EventAggregator();
        }

        [Test]
        public void Invalid_Input_Validation()
        {
            var mockedEventHandler = new Mock<Action<T>>();

            _eventAggregator.Subscribe(mockedEventHandler.Object);

            _eventAggregator.Publish<T>(null);

            mockedEventHandler.Verify(_ => _.Invoke(null), Times.Never);
        }

        [Test]
        public void Messages_Should_Be_Dispatched_To_Single_Observer()
        {
            var mockedEventHandler = new Mock<Action<T>>();

            var mockValue = new Mock<T>();

            _eventAggregator.Subscribe(mockedEventHandler.Object);

            _eventAggregator.Publish(mockValue.Object);

            mockedEventHandler.Verify(_ => _.Invoke(It.IsAny<T>()), Times.Once);
        }

        [Test]
        public void Messages_Should_Not_Be_Dispatched_To_UnSubscribed_Observer()
        {
            var mockedEventHandler = new Mock<Action<T>>();

            var mockValue = new Mock<T>();

            _eventAggregator.Subscribe(mockedEventHandler.Object);

            _eventAggregator.Publish(mockValue.Object);

            mockedEventHandler.Verify(_ => _.Invoke(It.IsAny<T>()), Times.Once);

            _eventAggregator.UnSubscribe(mockedEventHandler.Object);

            _eventAggregator.Publish(mockValue.Object);

            mockedEventHandler.Verify(_ => _.Invoke(It.IsAny<T>()), Times.Once);
        }
    }
}
