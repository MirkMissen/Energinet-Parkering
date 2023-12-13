using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;


namespace Case.Tests.Setup;

internal sealed class TestApp : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    public TestApp(Action<IServiceCollection>? services = null)
    {
        Client = WithWebHostBuilder(builder =>
        {
            if (services is not null)
            {
                builder.ConfigureServices(services);
            }

            builder.UseEnvironment("test");
        }).CreateClient();
    }
}