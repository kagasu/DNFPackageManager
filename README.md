# DNFPackageManager
package.lst parser for
- DNF(Dungeon & Fighter)
- DFO(Dungeon Fighter Online)
- Arad(アラド戦記)

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
```json
{
"DirectoryName": "ImagePacks2",
"Files": [
  {
    "FileName": "sprite_character_common_weaponavatar_3.NPK",
    "Unknown01": 1863,
    "FileSize": 3743,
    "Sha256HashValue": "Kg3p5xSrBA1kLNfOAkvG5NkKB8JkwQkSyzYMoPkVhBU="
  },
  {
    "FileName": "sprite_character_common_weaponavatar_rose13.NPK",
    "Unknown01": 14404,
    "FileSize": 16542,
    "Sha256HashValue": "fCqypiTqpiY1BB+OxU5GCaSfNRjGJf3cU0LYsZl3gzE="
  },
  {
    "FileName": "sprite_common_hiteffect_age 12.NPK",
    "Unknown01": 6845,
    "FileSize": 9891,
    "Sha256HashValue": "4hZE9eP02zQs6/oY3lVazb6uhTxj98Z/1S3solDMv/E="
  },
  {
    "FileName": "sprite_creature_2013kidssd.NPK",
    "Unknown01": 153945,
    "FileSize": 162861,
    "Sha256HashValue": "EEXgd6pjx6U5r82UycipWS8gWs685QqH/SN4n09hZ0c="
  },
  ...
```
