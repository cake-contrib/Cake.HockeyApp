Task("HockeyApp-Creates-New-Version")
    .IsDependentOn("Create-New-Apk-Version")
    .IsDependentOn("Create-New-Ipa-Version");

Task("HockeyApp-Upload")
    .IsDependentOn("Upload-Apk")
    .IsDependentOn("Upload-Ipa");