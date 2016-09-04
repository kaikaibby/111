using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorJS.Configuration;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.IO;
using System.Web.Hosting;

namespace RazorJS
{
    public class RazorJSFileParser
    {
        private object lockerObject = new object();
        private readonly string _filename, _fullUrl;

        public RazorJSFileParser(string filename)
        {
            _filename = filename;
        }

        public RazorJSFileParser(string filename, string fullUrl) : this(filename)
            //works for script with a query string
        {
            _fullUrl = fullUrl;
        }

        #region [ Public Methods ]


        public string ScriptInclude(bool useTags = true)
        {
            TagBuilder builder = BuildScriptTag(string.Format("{0}?fn={1}", 
                Extensions.ResolveUrl(RazorJSSettings.Settings.HandlerPath), Extensions.ResolveUrl(_filename)));
            return builder.ToString();
        }

        public string InlineScript<TModel>(TModel model, bool addScriptTags = true)
        {
            TagBuilder builder = BuildScriptTag();
            string filePath = GetFilePath(_filename);
            string template = GetJs(filePath);
            string result = ParseTemplateWithModel(template, model, filePath);
            if (!addScriptTags)
                return result;
            builder.InnerHtml = result;
            return builder.ToString();
        }

        public string InlineScript(bool addScriptTags = true)
        {
            TagBuilder builder = BuildScriptTag();
            string filePath = GetFilePath(_filename);
            string template = GetJs(filePath);
            string result = ParseTemplate(template, _fullUrl ?? filePath);
            if (!addScriptTags)
                return result;
            builder.InnerHtml = result;
            return builder.ToString();
        }

        #endregion

        #region [ Private Methods ]

        private static TagBuilder BuildScriptTag(string src = "")
        {
            TagBuilder builder = new TagBuilder("script");
            builder.Attributes["type"] = "text/javascript";
            if (!string.IsNullOrEmpty(src))
                builder.Attributes["src"] = src;
            return builder;
        }

        private string ParseTemplate(string template, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(template))
                    return string.Empty;

                var razorEngineconfig = new TemplateServiceConfiguration
                {
                    BaseTemplateType = typeof(HtmlTemplateBase)
                };

                using (var service = RazorEngineService.Create(razorEngineconfig))
                {
                    if (!CachedFileAccess.IsCompiled(name))
                    {
                        //protect from random crashes of the RazonEngine service compiler
                        lock (lockerObject)
                        {
                            service.Compile(template, name);
                            CachedFileAccess.SetCompiled(name);
                        }
                    }

                    return service.Run(name);
                }
            }
            catch (TemplateCompilationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var e in ex.CompilerErrors)
                    sb.AppendFormat("{0}\n", e.ToString().Replace(e.FileName, string.Empty));
                throw new JSFileParserException(string.Format("Failure to parse template {0}. See Errors:\n{1}", _filename, sb.ToString()));
            }
        }

        private string ParseTemplateWithModel<T>(string template, T model, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(template))
                    return string.Empty;
                var razorEngineconfig = new TemplateServiceConfiguration
                {
                    BaseTemplateType = typeof(HtmlTemplateBase<>)
                };

                using (var service = RazorEngineService.Create(razorEngineconfig))
                {
                    if (!CachedFileAccess.IsCompiled(name))
                    {
                        service.Compile(template, name, typeof(T));
                        CachedFileAccess.SetCompiled(name);
                    }
                    return service.Run(name, typeof(T));
                }
            }
            catch (RazorEngine.Templating.TemplateCompilationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var e in ex.CompilerErrors)
                    sb.AppendFormat("{0}\n", e.ToString().Replace(e.FileName, string.Empty));
                throw new JSFileParserException(string.Format("Failure to parse template {0}. See Errors:\n{1}", _filename, sb.ToString()));
            }
        }

        #region [ Static File Methods ]

        private static string GetFilePath(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return string.Empty;
            string exactFilename = HostingEnvironment.MapPath(filename);
            if (!IsValidFilename(exactFilename))
            {
                throw new JSFileParserException(string.Format("File '{0}' is invalid or was not found. Only files with .js extension are valid.", filename));
            }
            return exactFilename;
        }

        private static bool IsValidFilename(string filename)
        {
            if (!RazorJSSettings.Settings.AllowedPaths.Any() || RazorJSSettings.Settings.AllowedPaths
                .Any(config => filename.StartsWith(HostingEnvironment.MapPath(config.Path))))
            {
                return Path.GetExtension(filename) == ".js" && File.Exists(filename);
            }
            return false;
        }

        private static string GetJs(string filePath)
        {
            return CachedFileAccess.ReadAllText(filePath);
        }

        #endregion

        #endregion
    }
}