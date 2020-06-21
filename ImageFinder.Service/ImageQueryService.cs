
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
    public class ImageQueryService : IImageQueryService
    {
        private readonly ILogger<ImageQueryService> _logger;
        private readonly IMediator _mediator;

        public ImageQueryService(ILogger<ImageQueryService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IList<ImageMetadata>> QueryAsync(string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria)) throw new ArgumentException("Invalid Criteria Value.", nameof(criteria));

            try
            {
                _logger.LogInformation($"Image search criteria :: {criteria}");
                return await _mediator.Send(new ImageSearchQuery { Criteria = criteria });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network failure, Unable to perform search");
                throw new ImageQueryException(ex);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unknown failure, Unable to perform search");
                throw new ImageQueryException(ex);
            }
        }
    }
}
