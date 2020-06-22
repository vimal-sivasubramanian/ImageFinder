using ImageFinder.CrossCutting;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;

namespace ImageFinder.Presentation.ViewModels
{
    public class ImagePreviewViewModel : NotifyPropertyBase, IImagePreviewViewModel
    {
        private bool _isActivated;
        private ImageMetadata _imageMetadata;
        public bool IsActivated
        {
            get => _isActivated;
            set
            {
                _isActivated = value;
                OnPropertyChanged(nameof(IsActivated));
            }
        }

        public ImageMetadata ImageMetadata
        {
            get => _imageMetadata;
            set
            {
                _imageMetadata = value;
                OnPropertyChanged(nameof(ImageMetadata));
            }
        }

        public ImagePreviewViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<ImageMetadata>(OnImageSelectedForPreview);
        }

        private void OnImageSelectedForPreview(ImageMetadata selectedImage)
        {
            ImageMetadata = selectedImage;
            IsActivated = true;
        }
    }
}