using ImageFinder.CrossCutting.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageFinder.CrossCutting.Interfaces
{
    public interface ITweetQueryService
    {
        Task<IList<Tweet>> QueryAsync(string criteria);
    }
}
