using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using System;
using System.Windows.Input;

namespace ImageFinder.Presentation.Commands
{
    internal class OpenImageCommand : ICommand
    {
        private readonly IEventAggregator _eventAggregator;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public OpenImageCommand(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool CanExecute(object parameter) => parameter != null;

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            if (parameter is ImageMetadata imageMetadata)
                _eventAggregator.Publish(imageMetadata);
            else
                throw new ArgumentException(nameof(parameter));
        }
    }
}
