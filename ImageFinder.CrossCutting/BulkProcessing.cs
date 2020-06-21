using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using System;

namespace ImageFinder.CrossCutting
{
    public class BulkProcessing : IDisposable
    {
        private readonly IEventAggregator _eventAggregator;
        private BulkProcessing(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Publish(ProcessingState.Started);
        }

        public void Dispose()
        {
            _eventAggregator.Publish(ProcessingState.Completed);
        }

        public static IDisposable Start(IEventAggregator eventAggregator)
        {
            return new BulkProcessing(eventAggregator);
        }
    }
}
