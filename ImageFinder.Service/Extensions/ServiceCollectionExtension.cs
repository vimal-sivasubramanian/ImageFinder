using ImageFinder.CrossCutting.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


namespace ImageFinder.Service
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQueryService(this IServiceCollection services)
        {
            services.AddSingleton<IImageQueryService, ImageQueryService>()
                    .AddSingleton<HttpClient>()
                    .AddMediatR(typeof(ImageQueryService));
            return services;
        }
    }
}