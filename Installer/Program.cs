using System.Diagnostics;
using Installer;

var commandLineArguments = Environment.GetCommandLineArgs();

if (commandLineArguments.Length > 1)
{
    var processId = commandLineArguments[1];
    var folderPath = commandLineArguments[2];
    var excludeFolder = commandLineArguments[3];
    var rarFile = commandLineArguments[4];
    var deckBuilderExePath = commandLineArguments[5];

    using var process = Process.GetProcessById(int.Parse(processId));
    process.Kill();

    Fixture.Update(folderPath, excludeFolder, rarFile, out var tempFolder);
    
    Process.Start(deckBuilderExePath, tempFolder);
}