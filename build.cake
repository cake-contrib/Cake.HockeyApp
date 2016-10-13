#addin "Cake.Xamarin"

// ARGUMENTS
var target = Argument("target", "default");
var configuration = Argument("configuration", "release");

var local = BuildSystem.IsLocalBuild;

// Define directories.
var sln = "Cake.HockeyApp.sln";
var output = Directory("./output");

var releaseNotes = ParseReleaseNotes(File("./ReleaseNotes.md"));
var projectTitle = "Cake.HockeyApp";
var owner = "Paul Reichelt";
var authors = owner + " and contributors";
var copyright = "Copyright 2016 (c) " + authors;

// version
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

var prerelease = false;

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

        DotNetCorePack("./src/Cake.HockeyApp/", new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = "./output/artifacts/"
        });

      /*  NuGetPack(new NuGetPackSettings
        {
            Id                      = projectTitle,
            Version                 = prerelease ? semVersion : version,
            Title                   = projectTitle,
            Authors                 = new[] { authors },
            Owners                  = new[] { owner },
            Description             = "HockeyApp Addin for Cake Build Automation System",
            Summary                 = "The HockeyApp Addin for Cake allows you to upload you app package to HockeyApp",
            ProjectUrl              = new Uri("https://github.com/cake-contrib/Cake.HockeyApp"),
          //  IconUrl                 = new Uri("http://cdn.rawgit.com/SomeUser/TestNuget/master/icons/testnuget.png"),
            LicenseUrl              = new Uri("https://github.com/cake-contrib/Cake.HockeyApp/blob/master/LICENSE.md"),
            Copyright               = copyright,
          //  ReleaseNotes            = new [] {"Bug fixes", "Issue fixes", "Typos"},
            Tags                    = new [] {"Cake", "Script", "Build", "HockeyApp", "Deploment" },
            RequireLicenseAcceptance= false,
            Symbols                 = false,
            NoPackageAnalysis       = true,
            Files                   = new []
            {
                new NuSpecContent {Source = "Cake.HockeyApp.dll", Target = "net45"},
                new NuSpecContent {Source = "Cake.HockeyApp.xml", Target = "net45"},
                new NuSpecContent {Source = "Newtonsoft.Json.dll", Target = "net45"},
                new NuSpecContent {Source = "RestSharp.dll", Target = "net45"},
            },
            BasePath                = "./src/Cake.HockeyApp/bin/release",
            OutputDirectory         = artifacts
         }); */
    });

Task("Patch-Assembly-Info")
    .Does(() =>
    {
        CreateAssemblyInfo("./src/Cake.HockeyApp/Properties/AssemblyInfo.cs", new AssemblyInfoSettings {
            Product = projectTitle,
            Version = version,
            FileVersion = version,
            InformationalVersion = semVersion,
            Copyright = copyright
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