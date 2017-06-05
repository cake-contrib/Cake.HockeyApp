namespace Cake.HockeyApp.Tests
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Diagnostics;
    using Core.IO;
    using FakeItEasy;
    using Xunit;

    public class HockeyAppTests
    {
        ////[Fact]
        ////public void Can_upload_to_autodiscover()
        ////{
        ////    var log = A.Fake<ICakeLog>();
        ////    var context = A.Fake<ICakeContext>();

        ////    A.CallTo(() => context.Log).Returns(log);
        ////    A.CallTo(() => log.Write(A<Verbosity>._, A<LogLevel>._, A<string>._, A<object[]>._))
        ////        .Invokes(( Verbosity _,  LogLevel __, string format, object[] args) => Console.WriteLine(format, args));

        ////    var result = context.UploadToHockeyApp(new FilePath("C:/tmp/android.apk"), new HockeyAppUploadSettings()
        ////    {
        ////        ApiToken = "9ed5932bf6314dd2921abc38c6be6bde"
        ////    });

        ////    var url = result.PublicUrl;
        ////}
    }
}