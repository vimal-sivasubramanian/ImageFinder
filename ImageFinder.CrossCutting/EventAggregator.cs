using ImageFinder.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ImageFinder.CrossCutting
{
    public class EventAggregator : IEventAggregator
    {
        private readonly IList<Delegate> _handlers;

        private readonly SynchronizationContext _synchronizationContext;

        public EventAggregator()
        {
            _synchronizationContext = SynchronizationContext.Current;
            _handlers = new List<Delegate>();
        }

        public void Publish<T>(T message)
        {
            if (message == null)
            {
                return;
            }

            if (_synchronizationContext != null)
            {
                _synchronizationContext.Post(m => Dispatch((T)m), message);
            }
            else
            {
                Dispatch(message);
            }
        }

        public void Subscribe<T>(Action<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            _handlers.Add(eventHandler);
        }

        public void UnSubscribe<T>(Action<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            _handlers.Remove(eventHandler);
        }

        private void Dispatch<T>(T message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            foreach (var handler in _handlers.OfType<Action<T>>())
            {
                handler(message);
            }
        }
    }
}
