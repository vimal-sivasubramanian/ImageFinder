using ImageFinder.CrossCutting;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Presentation.Commands;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;

namespace ImageFinder.Presentation.ViewModels
{
    public class ImageFinderViewModel : NotifyPropertyBase, IImageFinderViewModel
    {
        private bool _isProcessing;

        public ICommand Search { get; private set; }

        public IImageListViewModel ImageListViewModel { get; set; }

        public ISnackbarMessageQueue AlertMessageQueue { get; set; }

        public bool IsProcessing { get => _isProcessing; set { _isProcessing = value; OnPropertyChanged(nameof(IsProcessing)); } }

        public IImagePreviewViewModel ImagePreviewViewModel { get; set; }

        public ImageFinderViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator)
        {
            Search = serviceProvider.GetService<SearchCommand>();

            ImageListViewModel = serviceProvider.GetService<IImageListViewModel>();

            ImagePreviewViewModel = serviceProvider.GetService<IImagePreviewViewModel>();

            AlertMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(10));

            SubscribeToEvents(eventAggregator);
        }

        private void SubscribeToEvents(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<ProcessingState>(state => IsProcessing = state.IsProcessing);

            eventAggregator.Subscribe<Error>(error => AlertMessageQueue.Enqueue(error.Message, "Ok", () => { }));
        }
    }
}
