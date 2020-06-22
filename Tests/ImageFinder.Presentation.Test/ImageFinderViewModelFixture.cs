using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Presentation.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFinder.Presentation.Test
{
    [TestFixture]
    class ImageFinderViewModelFixture
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        IImageFinderViewModel _fixture;

        [SetUp]
        public void Setup()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _eventAggregatorMock.Setup(_ => _.Subscribe(It.IsAny<Action<ProcessingState>>())).Callback<Action<ProcessingState>>(_ => _(ProcessingState.Started));

            _fixture = new ImageFinderViewModel(_serviceProviderMock.Object, _eventAggregatorMock.Object);
        }

        [Test]
        public void Should_Subscribe_For_Events()
        {
            _eventAggregatorMock.Verify(_ => _.Subscribe(It.IsAny<Action<ProcessingState>>()), Times.Once);
            _eventAggregatorMock.Verify(_ => _.Subscribe(It.IsAny<Action<Error>>()), Times.Once);
        }

        [Test]
        public void Should_Update_Locally_Maintained_Processing_State_Upon_Processing_Update()
        {
            Assert.IsTrue(_fixture.IsProcessing);
        }
    }
}
