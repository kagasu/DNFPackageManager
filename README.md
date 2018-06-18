# DNFPackageManager

### How to use
```cs
// Parse from package.lst file
IWebProxy proxy = null;
var parser = new PackageManager.PackageManager(proxy);

using (var stream = File.OpenRead("package.lst"))
{
  var packageInfo = parser.Parse(stream);
  File.WriteAllText("out.json", JsonConvert.SerializeObject(packageInfo, Formatting.Indented));
}
```

### out.json
![](https://i.imgur.com/K5ib07l.png)
