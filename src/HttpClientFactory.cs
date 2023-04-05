using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace XUnit.Extensions.IntegrationTests;

public sealed class HttpClientFactory
{
    private readonly IConfiguration _configuration;

    public HttpClientFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public HttpClient CreateClient()
    {
        return CreateClient(string.Empty);
    }

    public HttpClient CreateClient(string api_name)
    {
        return CreateClient(api_name, null);
    }

    public HttpClient CreateClient(string api_name, string? authorizationToken)
    {
        var handler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (_, _, _, policyErrors) =>
            {
                if (policyErrors != System.Net.Security.SslPolicyErrors.None)
                {
                    Console.WriteLine($"Error SSL: {policyErrors}");
                }

                return true;
            }
        };

        var keyName = string.IsNullOrEmpty(api_name) ? "url" : $"{api_name}_url";

        var baseUrl = _configuration[keyName];
        if (baseUrl == null)
            throw new KeyNotFoundException($"Impossible to find the key {keyName}");

        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };

        if (!string.IsNullOrEmpty(authorizationToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                authorizationToken
            );
        }

        return httpClient;
    }
}
