using ImageFinder.CrossCutting;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ImageFinder.Presentation.ViewModels
{
    public class ImageListViewModel : NotifyPropertyBase, IImageListViewModel
    {
        public ICommand OpenImage { get; set; }

        public IList<ImageMetadata> Images { get; set; }

        public ImageListViewModel(IEventAggregator eventAggregator, IServiceProvider serviceProvider)
        {
            OpenImage = serviceProvider.GetService<OpenImageCommand>();
            SubscribeToEvents(eventAggregator);
        }

        private void SubscribeToEvents(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<IList<ImageMetadata>>(metadata => { Images = metadata; OnPropertyChanged(nameof(Images)); });
            eventAggregator.Subscribe<Error>(error => { Images = null; OnPropertyChanged(nameof(Images)); });
        }
    }
}
