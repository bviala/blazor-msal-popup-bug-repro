using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorJuiceAdmin;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


/* 
    MSAL Authentication service support
    https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-microsoft-entra-id?view=aspnetcore-7.0#authentication-service-support-1
*/
builder.Services.AddHttpClient("TEST", client => 
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("TEST"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    // options.ProviderOptions.DefaultAccessTokenScopes.Add("api://{{ API app reg ID }}/API.Access");
});


await builder.Build().RunAsync();