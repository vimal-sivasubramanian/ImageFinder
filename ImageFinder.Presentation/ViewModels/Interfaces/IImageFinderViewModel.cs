using MaterialDesignThemes.Wpf;
using System.Windows.Input;

namespace ImageFinder.Presentation.ViewModels
{
    public interface IImageFinderViewModel
    {
        ICommand Search { get; }
        IImageListViewModel ImageListViewModel { get; set; }
        ISnackbarMessageQueue AlertMessageQueue { get; set; }
        bool IsProcessing { get; set; }
        IImagePreviewViewModel ImagePreviewViewModel { get; set; }
    }
}