using System;

namespace ImageFinder.CrossCutting.Interfaces
{
    public interface IEventAggregator
    {
        void Publish<T>(T message);
        void Subscribe<T>(Action<T> eventHandler);
        void UnSubscribe<T>(Action<T> eventHandler);
    }
}