using ImageFinder.CrossCutting.Models;
using MediatR;
using System.Collections.Generic;

namespace ImageFinder.Service.Queries
{
    internal class TweetSearchQuery : IRequest<IList<Tweet>>
    {
        public string Criteria { get; set; }
    }
}
