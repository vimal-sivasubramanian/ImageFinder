using ImageFinder.CrossCutting.Models;
using System.Collections.Generic;

namespace ImageFinder.Presentation.ViewModels
{
    public interface IImageListViewModel
    {
        IList<ImageMetadata> Images { get; set; }
    }
}