using ImageFinder.CrossCutting.Models;
using ImageFinder.Service.Queries;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImageFinder.Service.Handlers
{
    internal class TwitterSearchHandler : IRequestHandler<TweetSearchQuery, IList<Tweet>>
    {
        public Task<IList<Tweet>> Handle(TweetSearchQuery request, CancellationToken cancellationToken)
        {
            IList<Tweet> tweets = Enumerable.Repeat(new Tweet { Message = "Random", Author = "Vimal" }, 20).ToList();
            return Task.FromResult(tweets);
        }
    }
}
