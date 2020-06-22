using ImageFinder.CrossCutting.Models;
using System.Windows.Input;

namespace ImageFinder.Presentation.ViewModels
{
    public interface IImagePreviewViewModel
    {
        ImageMetadata ImageMetadata { get; set; }
        bool IsActivated { get; set; }
    }
}
