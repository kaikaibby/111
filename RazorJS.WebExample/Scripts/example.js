@using System.IO;

var s = 'Hello at @DateTime.Now \n @File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/web.config"))';
s += 'from @Href("~/Models/Test")';
alert(s);
alert('@Url.Action("Index")')