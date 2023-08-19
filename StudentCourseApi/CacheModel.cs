using Microsoft.Extensions.Caching.Memory;

namespace StudentCourseApi
{
    public static class CacheModel<T>
    {
        private static readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        public static T Get(string key)
        {
            if (memoryCache.TryGetValue(key, out T results))
                return results;
            else
                return default;
        }
        public static void Set(string key, T value)
        {

            var options = new MemoryCacheEntryOptions();
            memoryCache.Set(key, value);
        }
        public static void Delete(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
