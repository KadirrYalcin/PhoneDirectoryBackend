using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

namespace PhoneDirectory.MiddleWares
{
    public static class AddPhotoMiddleWaes
    {
        public static WebApplication UseCdnStaticFiles(this WebApplication app, string rootPath)
        {
            string ResourcePath = Path.Combine(Directory.GetCurrentDirectory(), rootPath);
            if (!Directory.Exists(ResourcePath))
            {
                Directory.CreateDirectory(ResourcePath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(ResourcePath),
                RequestPath = "/uploads",
                HttpsCompression = HttpsCompressionMode.Compress,
                ServeUnknownFileTypes = true,
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(30)
                    };
                }
            });

        
            return app;
        }
    }
}
