using RedditStoreApp.Data.Model;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Factory
{
    public interface IRedditApi
    {
        bool IsLoggedIn { get; }
        Task<bool> LoginAsync(string userName, string password);
        Task<Listing<Subreddit>> GetMySubredditsListAsync();
        Task<Listing<Subreddit>> GetPopularSubredditsListAsync();
    }
}
