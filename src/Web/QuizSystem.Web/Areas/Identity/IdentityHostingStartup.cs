using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(QuizSystem.Web.Areas.Identity.IdentityHostingStartup))]

namespace QuizSystem.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
