using ImageFinder.Presentation.ViewModels;
using System.Windows;

namespace ImageFinder.Presentation.Views
{
    /// <summary>
    /// Interaction logic for ImageFinderView.xaml
    /// </summary>
    public partial class ImageFinderView : Window
    {
        public ImageFinderView(IImageFinderViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
