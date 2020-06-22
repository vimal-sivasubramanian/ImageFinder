using ImageFinder.CrossCutting;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.Presentation.Commands;
using ImageFinder.Presentation.ViewModels;
using ImageFinder.Presentation.Views;
using ImageFinder.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Windows;

namespace ImageFinder.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            services.AddSingleton<IEventAggregator, EventAggregator>()
                    .AddTransient<IImageListViewModel, ImageListViewModel>()
                    .AddTransient<IImageFinderViewModel, ImageFinderViewModel>()
                    .AddTransient<IImagePreviewViewModel, ImagePreviewViewModel>()
                    .AddTransient<SearchCommand>()
                    .AddTransient<OpenImageCommand>()
                    .AddTransient<ImageFinderView>()
                    .AddQueryService()
                    .AddLogging(builder => builder.AddSerilog(logger));
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var view = _serviceProvider.GetService<ImageFinderView>();
            view.Show();
        }
    }
}
