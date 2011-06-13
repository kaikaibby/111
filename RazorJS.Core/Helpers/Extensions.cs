using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Web;

namespace RazorJS
{
    internal static class Extensions
    {
        static readonly Control _dummy = new Control();
        public static string ResolveUrl(string url)
        {
            if (url == null)
                return null;
            return url.StartsWith("~") ? VirtualPathUtility.ToAbsolute(url) : _dummy.ResolveUrl(url);
        }
    }
}
