using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace RazorJS.Configuration
{
    public class RazorJSSettings : System.Configuration.ConfigurationSection
    {
        private static RazorJSSettings settings = null;
        public static RazorJSSettings Settings
        {
            get
            {
                if (settings == null)
                    settings = ConfigurationManager.GetSection("razorJSSettings") as RazorJSSettings;
                return settings ?? new RazorJSSettings();
            }
        }

        IEnumerable<AllowedPathElement> _allowedPaths;
        public IEnumerable<AllowedPathElement> AllowedPaths
        {
            get
            {
                if (_allowedPaths == null)
                {
                    var items = AllowedPathInternal;
                    if (items != null)
                        _allowedPaths = items.OfType<AllowedPathElement>();
                    else
                        _allowedPaths = new List<AllowedPathElement>();
                }
                return _allowedPaths;
            }
        }


        [ConfigurationProperty("allowedPaths", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AllowedPathCollection))]
        protected AllowedPathCollection AllowedPathInternal
        {
            get { return this["allowedPaths"] as AllowedPathCollection; }
        }

        [ConfigurationProperty("handlerPath", DefaultValue = "~/razorjs.axd")]
        //[RegexStringValidator("^~/(.*)")]
        public string HandlerPath
        {
            get { return (string)this["handlerPath"]; }
            set { this["handlerPath"] = value; }
        }
    }
}
