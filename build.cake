#addin "Cake.Xamarin"

// ARGUMENTS
var target = Argument("target", "default");
var configuration = Argument("configuration", "release");

// Define directories.
var sln = "Cake.HockeyApp.sln";
var report = Directory("./reports");
var output = Directory("./output");
var src = "./src/*";

// TASKS
Task("clean")
    .Does(() =>
    {
        CleanDirectories(string.Format("./src/**/obj/{0}", configuration));
        CleanDirectories(string.Format("./src/**/bin/{0}", configuration));
    });

Task("restore")
    .Does(() => DNURestore(src));

Task("build")
    .Does(() => DNUBuild(src));
    
Task("rebuild")
    .IsDependentOn("clean")
    .IsDependentOn("build");

// default target is build
Task("default")
    .IsDependentOn("build");
    
Task("pack")
    .Does(() => DNUPack(src, new DNUPackSettings 
    { 
        OutputDirectory = "./output/artifacts", 
        Configurations = new [] {configuration}
    }));

// EXECUTION
RunTarget(target);