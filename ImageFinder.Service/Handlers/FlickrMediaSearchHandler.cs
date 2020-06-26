using ImageFinder.CrossCutting.Models;
using ImageFinder.Service.Models;
using ImageFinder.Service.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace ImageFinder.Service.Handlers
{
    internal class FlickrMediaSearchHandler : IRequestHandler<ImageSearchQuery, IList<ImageMetadata>>
    {
        private const string _flickApi = "https://www.flickr.com/services/feeds/photos_public.gne";
        private const string _token = "img src=\"";
        private readonly HttpClient _httpClient;
        private readonly ILogger<FlickrMediaSearchHandler> _logger;

        public FlickrMediaSearchHandler(HttpClient httpClient, ILogger<FlickrMediaSearchHandler> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IList<ImageMetadata>> Handle(ImageSearchQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _logger.LogDebug($"Image Search request with criteria :: {request.Criteria}  was received by Flickr Handler");

            var response = await _httpClient.GetAsync($"{ _flickApi }?tags={ request.Criteria }");
            _logger.LogDebug($"FLickr Api responded with status code :: {response.StatusCode}");
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();

            if (stream.Length == 0) return default;

            var feed = (FlickrFeed)new XmlSerializer(typeof(FlickrFeed)).Deserialize(stream);
            var result = feed.Entries
                    .Select(MapToImageMetadata)
                    .ToList();
            return result;
        }

        private ImageMetadata MapToImageMetadata(Entry entry)
        {
            string thumbnailUrl = null;
            try
            {
                var contentXml = entry.Content.ConvertHtmlToXml("content");
                thumbnailUrl = contentXml.Substring(contentXml.IndexOf(_token) + _token.Length).Split('\"')[0];
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to extract thumbnail url from " + entry.Content);
            }

            var url = entry.Links.Find(link => link.Type == "image/jpeg").Href;

            return new ImageMetadata()
            {
                Title = entry.Title,
                Url = url,
                ThumbnailUrl = thumbnailUrl ?? url,
                Author = entry.Author?.Name ?? "Unknown"
            };
        }
    }
}