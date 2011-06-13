using System.Web.Mvc;
using RazorEngine.Templating;
using System.Web;

namespace RazorJS
{
    public class HtmlTemplateBase : TemplateBase
    {
        public HtmlTemplateBase()
        {
            Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }
        public string Href(string originalUrl)
        {
            return Extensions.ResolveUrl(originalUrl);
        }

        public UrlHelper Url { get; set; }
    }
    public class HtmlTemplateBase<TModel> : HtmlTemplateBase, ITemplate<TModel>
    {
        public TModel Model { get; set; }
    }
}
