using ImageFinder.CrossCutting.Exceptions;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using ImageFinder.Service.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageFinder.Service
{
    public class TweetQueryService : ITweetQueryService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TweetQueryService> _logger;

        public TweetQueryService(IMediator mediator, ILogger<TweetQueryService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IList<Tweet>> QueryAsync(string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria)) throw new ArgumentException("Invalid Criteria Value.", nameof(criteria));

            try
            {
                _logger.LogInformation($"Tweet search criteria :: {criteria}");
                return await _mediator.Send(new TweetSearchQuery { Criteria = criteria });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network failure, Unable to perform search");
                throw new TweetQueryException(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown failure, Unable to perform search");
                throw new TweetQueryException(ex);
            }
        }
    }
}
