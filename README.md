ConsoleControl
==============

ConsoleControl is a C# class library that lets you embed a console in a WinForms or WPF application. This console can be used for input and output for a process. It's great for making tools and utilities.


![ConsoleControl Screenshot](https://github.com/dwmkerr/consolecontrol/blob/master/Assets/screenshot.png?raw=true "ConsoleControl Screenshot")

Installing ConsoleControl
-------------------------

Installing couldn't be easier, just use Nuget. You can search for 'ConsoleControl' or install directly.

For WinForms:

````
PM> Install-Package ConsoleControl
````

For WPF:

````
PM> Install-Package ConsoleControl.WPF
````

Using ConsoleControl
--------------------

Once you've installed the package, the ConsoleControl will be available in the toolbox. Add a ConsoleControl to your WPF or WinForms project and call 'StartProcess' to start a new process. The process will run and all output will be directed to the ConsoleControl. You can also optionally enable input from the control.

Buidling and Extending the Code
-------------------------------

You can learn how the ConsoleControl was created by reading this article on the CodeProject:

http://www.codeproject.com/Articles/335909/Embedding-a-Console-in-a-C-Application

Building the code is as straightforward as downloading it and building the project.
