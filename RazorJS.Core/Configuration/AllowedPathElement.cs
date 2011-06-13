using System.Configuration;

namespace RazorJS.Configuration
{
    public class AllowedPathElement : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }
}
