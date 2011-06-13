using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace RazorJS
{
    /// <summary>
    /// Provides cached read access to small, frequently used files.
    /// Uses HostingEnvironment.Cache (same as app cache).
    /// <see cref="http://nathanaeljones.com/153/performance-killer-disk-io"/>
    /// </summary>
    internal class CachedFileAccess
    {
        private const string FILECACHEKEY = "{0}compiled";

        #region [ Private Methods ]
        
        private static string ReadAllText(string file, Encoding encoding)
        {
            if (HostingEnvironment.Cache == null) throw new InvalidOperationException("HostingEnvironment.Cache is null");
            string key = GetCacheKey(file, encoding);
            var value = HostingEnvironment.Cache[key] as string;
            if (value == null)
            {
                HostingEnvironment.Cache.Insert(string.Format(FILECACHEKEY, key), false);
                value = System.IO.File.ReadAllText(file, encoding);
                HostingEnvironment.Cache.Add(key, value, new System.Web.Caching.CacheDependency(file),
                    System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration,
                    System.Web.Caching.CacheItemPriority.Low, null);
            }
            return value;
        }

        private static string GetCacheKey(string file, Encoding encoding)
        {
            return string.Format("cached_file({0})_{1}", encoding.EncodingName, file.ToLowerInvariant().GetHashCode());
        }

        #endregion

        #region [ Public Methods ]

        public static void SetCompiled(string file)
        {
            string key = string.Format(FILECACHEKEY, GetCacheKey(file, Encoding.UTF8));
            HostingEnvironment.Cache[key] = true;
        }

        public static bool IsCompiled(string file)
        {
            string key = string.Format(FILECACHEKEY, GetCacheKey(file, Encoding.UTF8));
            bool changed = HostingEnvironment.Cache.Get(key) as bool? ?? true;
            return changed;
        }

        public static string ReadAllText(string file)
        {
            return ReadAllText(file, Encoding.UTF8);
        }

        #endregion

     }
}
