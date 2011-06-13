using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using RazorJS;
using RazorJS.Configuration;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString RazorJSInclude(this HtmlHelper html, string filename)
        {
            RazorJSFileParser eng = new RazorJSFileParser(filename);
            return html.Raw(eng.ScriptInclude());
        }

        public static IHtmlString RazorJSInline<TModel>(this HtmlHelper<TModel> html, string filename, TModel model, bool addScriptTags = true)
        {
            return html.Raw(new RazorJSFileParser(filename).InlineScript(model, addScriptTags));
        }

        public static IHtmlString RazorJSInline(this HtmlHelper html, string filename, bool addScriptTags = true)
        {
            return html.Raw(new RazorJSFileParser(filename).InlineScript(addScriptTags));
        }
    }
}
