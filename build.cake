// ARGUMENTS
var target = Argument("target", "default");
var configuration = Argument("configuration", "release");
var isPrerelease = HasArgument("pre");

var local = BuildSystem.IsLocalBuild;

// Define directories.
var sln = "Cake.HockeyApp.sln";
var output = Directory("./output");

var releaseNotes = ParseReleaseNotes(File("./ReleaseNotes.md"));
var projectTitle = "Cake.HockeyApp";

// version
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version + (isPrerelease ? "-pre" : "") : (version + string.Concat("-build-", buildNumber));

var prerelease = isPrerelease;

Setup(() =>
{
    if(!local)
        Information(string.Format("Building {0} Version: {1} on branch {2}", projectTitle, semVersion, AppVeyor.Environment.Repository.Branch));
    else
        Information(string.Format("Building {0} Version: {1}", projectTitle, semVersion));
});

// TASKS
Task("Build")
    .Does(() =>
    {
        DotNetCoreBuild("./src/Cake.HockeyApp", new DotNetCoreBuildSettings
        {
            Configuration = configuration
        });
    });

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(output);
        CleanDirectories(string.Format("./src/**/obj/{0}", configuration));
        CleanDirectories(string.Format("./src/**/bin/{0}", configuration));
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Pack")
    .IsDependentOn("Upload-To-NuGet");

Task("Pack")
    .Does(() =>
    {
        var artifacts = output + Directory("artifacts");
        CreateDirectory(artifacts);

        /* DotNetCorePack("./src/Cake.HockeyApp/", new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = "./output/artifacts/"
        }); */

        // Until cake/cake-build#787 is fixed we need to stay with *.nuspec files.

        NuGetPack("./Cake.HockeyApp.nuspec", new NuGetPackSettings
        {
            Version                 = prerelease ? semVersion : version,
            OutputDirectory         = artifacts
        });
    });

Task("Patch-Assembly-Info")
    .Does(() =>
    {
        CreateAssemblyInfo("./src/Cake.HockeyApp/Properties/AssemblyInfo.cs", new AssemblyInfoSettings {
            Product = projectTitle,
            Version = version,
            FileVersion = version,
            InformationalVersion = semVersion,
            Copyright = "Copyright 2016 (c) Paul Reichelt and contributors"
        });
    });

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");

Task("Restore")
    .Does(() => DotNetCoreRestore());

Task("Upload-To-NuGet")
    .WithCriteria(() => false);

// EXECUTION
RunTarget(target);