using System;
using System.Web;
using RazorEngine;
using RazorEngine.Templating;

namespace RazorJS
{
    public class RazorJSHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext _context)
        {
            _context.Response.Clear();
            _context.Response.ContentType = "text/javascript";
            string filename = _context.Request.QueryString["fn"];
            string template = new RazorJSFileParser(filename).InlineScript(false);
            _context.Response.Write(template);

            _context.Response.End();
        }

        #endregion
    }

}
