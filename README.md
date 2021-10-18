# MyAutoUpdater

## Project Introduction
* MyAutoUpdater is updated for fixing some bugs and optimizing some functions based on an open source project named "AutoUpdater.NET".Please visit [https://github.com/ravibpatel/AutoUpdater.NET](https://github.com/ravibpatel/AutoUpdater.NET) for its other descriptions.
* MyAutoUpdater is an auto updater program which is based on Windows and .NET Framework 2.0+,developed on Visual Studio 2015,designed for Windows devices, depending on .NET Framework 2.0+,and used for auto updating for applications on Windows devices.
* MyAutoUpdater is a serverless and independent client program which is called by third party applications via process calling with command line args and it's non-intrusive to third party applications.
* MyAutoUpdater uses preformatted XML file to store updating parameters.
* Nuget package name:Joeries.MyAutoUpdater.

## Command Line Parameters
* The way to run MyAutoUpdater is that you could call process with 5 command line parameters;
* 1st MainExeName:third party application's name for displaying in the winforms of MyAutoUpdater;
* 2nd CurVersion:third party application's current version number(like 1.5.2.123) for comparing with its latest version number to determine upgrading or not;
* 3rd UpdaterUrl:third party application's xml file url(only for HTTP) storing updating params for called by MyAutoUpdater (XML's format follows below);
* 4th MainExePath:third party application's main executable file path for auto running after dealed;
* 5th Silent:if upgrading silent or not(Optional Value:true/false);
* Code by C# follows:

```
using System.Diagnostics;

string startupPath = Application.StartupPath;//MyAutoUpdater's startupPath(usually the same as third party application's root folder path)
string updaterFile = Path.Combine(startupPath, "MyAutoUpdater.exe");//MyAutoUpdater's file path
string mainExeName = "Demo";//third party application's name
string curVersion = "1.5.2.123";//third party application's current version number
string updaterUrl = "http://www.demo.com/updater.xml";//third party application's updating XML file URL
string mainExePath = Path.Combine(startupPath, "demo.exe");//third party application's main executable file path
string silent = "true";//if upgrading silent or not

ProcessStartInfo processInfo = new ProcessStartInfo();
processInfo.FileName = updaterFile;
processInfo.WorkingDirectory = startupPath;
processInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", mainExeName, curVersion, updaterUrl, mainExePath, silent);

Process process = new Process();
process.StartInfo = processInfo;
process.Start();
```
* MyAutoUpdater is suitable to called in the init stage of third party application and the third party application should exit after calling.

## XML Format
* XML format like below:

```
<?xml version="1.0" encoding="UTF-8"?>
<item>
    <version>1.5.2.123</version>
    <url>http://www.demo.com/updater.zip</url>
    <changelog>http://www.demo.com/updater.txt</changelog>
    <mandatory>true</mandatory>
</item>
```

* version:third party application's latest version number;
* url:hird party application's packing file URL(only for HTTP) ending with .zip or .exe;
* changelog:url of changelog file(not implemented);
* mandatory:if necessary to upgrade(not implemented);
* MyAutoUpdater will unpack it and cover old files if .zip suffix and run it if .exe suffix.
