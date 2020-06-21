using ImageFinder.CrossCutting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageFinder.CrossCutting.Interfaces
{
    public interface IImageQueryService
    {
        Task<IList<ImageMetadata>> QueryAsync(string criteria);
    }
}