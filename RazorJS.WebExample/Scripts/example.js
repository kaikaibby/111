var s = 'Hello at @DateTime.Now \n';
s += 'from @Href("~/Models/Test")';
alert(s);
alert('@Url.Action("Index")')