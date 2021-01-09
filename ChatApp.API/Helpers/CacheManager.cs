using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using WebGrease;

namespace ChatApp.API
{
    /// <summary>
    /// Cache Manager
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        private readonly IMemoryCache _memoryCache;
         
        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// sets the cache entry T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationInMinutes"></param>
        /// <returns></returns>
        public T Set<T>(object key, T value, int expirationInMinutes = 60)
        {
            var options = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.Normal).SetAbsoluteExpiration(TimeSpan.FromMinutes(expirationInMinutes));
            options.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
            _memoryCache.Set(key, value, options);
            return value;
        }
        /// <summary>
        /// checks for cache entry existence
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(object key)
        {
            return _memoryCache.TryGetValue(key, out object result);
        }
        /// <summary>
        /// returns cache entry T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(object key)
        {
            return _memoryCache.TryGetValue(key, out T result) ? result : default(T);
        }

        /// <summary>
        /// clear cache entry
        /// </summary>
        /// <param name="key"></param>
        public void Clear(object key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// expires cache entries T based on CancellationTokenSource cancel 
        /// </summary>
        public void Reset()
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested &&
                _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();
        }

        #region NOT IMPLEMENTED
        public ICacheSection CurrentCacheSection => throw new NotImplementedException();

        public IDictionary<string, ReadOnlyCacheSection> LoadedCacheSections => throw new NotImplementedException();

        public string RootPath => throw new NotImplementedException();

        public ICacheSection BeginSection(WebGreaseSectionKey webGreaseSectionKey, bool autoLoad = true)
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }

        public void EndSection(ICacheSection cacheSection)
        {
            throw new NotImplementedException();
        }

        public string GetAbsoluteCacheFilePath(string category, string fileName)
        {
            throw new NotImplementedException();
        }

        public void SetContext(IWebGreaseContext newContext)
        {
            throw new NotImplementedException();
        }

        public string StoreInCache(string cacheCategory, ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public void LockedFileCacheAction(string lockFileContent, Action action)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
