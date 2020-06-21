using ImageFinder.CrossCutting.Models;
using MediatR;
using System.Collections.Generic;

namespace ImageFinder.Service.Queries
{
    internal class ImageSearchQuery : IRequest<IList<ImageMetadata>>
    {
        public string Criteria { get; set; }
    }
}