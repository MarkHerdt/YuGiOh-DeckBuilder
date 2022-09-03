using Github;

Fixture.CreateRarFile();

await Fixture.DeleteLatestReleaseAsync();
await Fixture.CreateNewReleaseAsync();
await Fixture.UploadLastReleaseBuildAsync();
await Fixture.UploadLastUpdateInfoAsync();