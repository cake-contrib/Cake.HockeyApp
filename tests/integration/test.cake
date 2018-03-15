// Utilities
#load "./utilities/resources.cake"
#load "./utilities/settings.cake"
#load "./utilities/xunit.cake"
#load "./HockeyApp/ios.cake"
#load "./HockeyApp/android.cake"

// Tests
#load "./HockeyApp/HockeyAppAliases.cake"

// References
#reference "System.Runtime"

//////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////

var target = Argument<string>("target", "Run-All-Tests");

//////////////////////////////////////////////////
// SETUP / TEARDOWN
//////////////////////////////////////////////////

Setup(ctx =>
{
});

Teardown(ctx =>
{
});

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Cake.HockeyApp")
    .IsDependentOn("HockeyApp-Creates-New-Version")
    .IsDependentOn("HockeyApp-Upload")
    .IsDependentOn("Upload-With-Upload-Url");

Task("Run-All-Tests")
    .IsDependentOn("Cake.HockeyApp");

//////////////////////////////////////////////////

RunTarget(target);