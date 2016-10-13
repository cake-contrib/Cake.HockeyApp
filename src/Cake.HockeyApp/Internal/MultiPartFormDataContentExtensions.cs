namespace Cake.HockeyApp.Internal
{
    using System.Net.Http;

    public static class MultiPartFormDataContentExtensions
    {
        public static void AddIfNotEmpty(this MultipartFormDataContent content, string name, string value)
        {
            if(!string.IsNullOrEmpty(value))
                content.Add(new StringContent(value), name);
        }
    }
}
