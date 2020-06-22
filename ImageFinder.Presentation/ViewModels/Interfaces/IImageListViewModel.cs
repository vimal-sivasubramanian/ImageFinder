using ImageFinder.CrossCutting.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace ImageFinder.Presentation.ViewModels
{
    public interface IImageListViewModel
    {
        ICommand OpenImage { get; set; }
        IList<ImageMetadata> Images { get; set; }
    }
}