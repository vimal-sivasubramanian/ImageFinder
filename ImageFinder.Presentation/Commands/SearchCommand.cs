using ImageFinder.CrossCutting;
using ImageFinder.CrossCutting.Exceptions;
using ImageFinder.CrossCutting.Interfaces;
using ImageFinder.CrossCutting.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Input;

namespace ImageFinder.Presentation.Commands
{
    public class SearchCommand : ICommand
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<SearchCommand> _logger;
        private readonly IImageQueryService _queryService;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public SearchCommand(IEventAggregator eventAggregator, ILogger<SearchCommand> logger, IImageQueryService queryService)
        {
            _eventAggregator = eventAggregator;
            _logger = logger;
            _queryService = queryService;
        }

        public bool CanExecute(object parameter) => !string.IsNullOrWhiteSpace(parameter?.ToString());

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            var criteria = parameter.ToString();

            _logger.LogDebug($"Initiating image search with criteria :: {criteria}");
            try
            {
                using (BulkProcessing.Start(_eventAggregator))
                {
                    var images = await _queryService.QueryAsync(criteria);
                    _logger.LogDebug($"Image search with criteria :: {criteria} resulted in { images?.Count ?? 0 } + images");
                    _eventAggregator.Publish(images);
                }
            }
            catch (ImageQueryException ex)
            {
                _logger.LogError(ex, "Network failure, Unable to perform search");
                _eventAggregator.Publish(new Error { Message = "Network failure, Unable to perform search" });
            }
        }
    }
}
