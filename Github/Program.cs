using Github;

var commandLineArguments = Environment.GetCommandLineArgs();
var commandLineArgument = commandLineArguments.Length > 1 ? commandLineArguments[1] : Fixture.Debug;

await Fixture.CreateRarFileAsync(commandLineArgument);

if (commandLineArgument == Fixture.Release)
{
    await Fixture.DeleteLatestReleaseAsync();
    await Fixture.CreateNewReleaseAsync();
    await Fixture.UploadLastReleaseBuildAsync();
    await Fixture.UploadLastUpdateInfoAsync();   
}