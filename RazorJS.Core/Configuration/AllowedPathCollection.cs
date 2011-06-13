using System.Configuration;

namespace RazorJS.Configuration
{
    public class AllowedPathCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AllowedPathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AllowedPathElement)element).Path;
        }
    }
}
