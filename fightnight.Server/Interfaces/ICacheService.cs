namespace fightnight.Server.Interfaces
{
    public interface ICacheService
    {
        Task AddToCacheAsync(string key, object value);
        // Its usually a string being returned
        Task<string> GetFromCacheAsync(string key);
        Task RemoveFromCacheAsync(string key);
    }
}
