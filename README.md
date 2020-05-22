# ConsoleControl

[![Build status](https://ci.appveyor.com/api/projects/status/8i3y31o3j6aomjim?svg=true)](https://ci.appveyor.com/project/dwmkerr/consolecontrol) [![GuardRails badge](https://badges.guardrails.io/dwmkerr/consolecontrol.svg?token=569f2cc38a148f785f3a38ef0bcf5f5964995d7ca625abfad9956b14bd06ad96&provider=github)](https://dashboard.guardrails.io/gh/dwmkerr/16697)

ConsoleControl is a C# class library that lets you embed a console in a WinForms or WPF application. This console can be used for input and output for a process. It's great for making tools and utilities.

![ConsoleControl Screenshot](./docs/screenshot.png "ConsoleControl Screenshot")

## Installing

Installing couldn't be easier, just use NuGet. You can search for 'ConsoleControl' or install directly.

For WinForms:

```
PM> Install-Package ConsoleControl
```

For WPF:

```
PM> Install-Package ConsoleControl.WPF
```

## Using ConsoleControl

Once you've installed the package, the ConsoleControl will be available in the toolbox. Add a ConsoleControl to your WPF or WinForms project and call `StartProcess` to start a new process. The process will run and all output will be directed to the ConsoleControl. You can also optionally enable input from the control.

## Developer Guide

To build, open the main `./source/ConsoleControl.sln` solution to build and run any of the code or samples.

You can also use the following scripts to run the processes:

| Script      | Notes                                     |
|-------------|-------------------------------------------|
| `build.ps1` | Build the solution from the command line. |

You can learn how the ConsoleControl was created by reading the article [Embedding a Console in a C# Application](http://www.codeproject.com/Articles/335909/Embedding-a-Console-in-a-C-Application) article on the CodeProject.

### Creating a Release

To create a release:

1. Update the version number in [`SharedAssemblyInfo.cs`](./source/SharedAssemblyInfo.cs)
2. Create a new version tag, then push `git push --follow-tags`

AppVeyor will build and publish a new NuGet package and as long as a new semver tag is pushed.
