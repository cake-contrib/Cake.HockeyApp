#addin "Cake.Xamarin"

#addin "Cake.ReSharperReports"
#tool "JetBrains.ReSharper.CommandLineTools"
#tool "ReSharperReports"

// ARGUMENTS
var target = Argument("target", "default");
var configuration = Argument("configuration", "release");

// Define directories.
var sln = "Cake.HockeyApp.sln";
var report = Directory("./reports");
var output = Directory("./output");

// TASKS
Task("clean")
    .Does(() =>
    {
        CleanDirectories(string.Format("./src/**/obj/{0}", configuration));
        CleanDirectories(string.Format("./src/**/bin/{0}", configuration));
    });

Task("restore")
    .Does(() => NuGetRestore(sln));

Task("build")
    .Does(() =>
    {
        if(IsRunningOnWindows())
            MSBuild(sln, cfg => cfg.Configuration = configuration);
        else
        // xbuild is crap we need to use the xamarin tool chain
            MDToolBuild(sln, cfg => cfg.Configuration = configuration);
    });
    
Task("rebuild")
    .IsDependentOn("clean")
    .IsDependentOn("build");

// default target is build
Task("default")
    .IsDependentOn("build");

// EXECUTION
RunTarget(target);