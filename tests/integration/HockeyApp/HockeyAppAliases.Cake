Task("HockeyApp-Creates-New-Version")
    .IsDependentOn("Create-New-Apk-Version");

Task("HockeyApp-Upload")
    .IsDependentOn("Upload-Apk");

Task("Upload-With-Upload-Url")
    .IsDependentOn("Upload-Apk-With-Upload-Url");
