using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace XUnit.Extensions.IntegrationTests;

public abstract class TestContext : IDisposable
{
    private readonly IServiceCollection _services;
    private IServiceProvider? _serviceProvider;
    private ITestOutputHelper? _testOutputHelper;
    private bool _disposedValue;

    protected TestContext()
    {
        _services = new ServiceCollection();
        Configuration = GetConfigurationRoot();
        AddServices(_services, Configuration);
    }

    protected abstract void AddServices(IServiceCollection services, IConfiguration? configuration);
    protected abstract IEnumerable<string> GetSettingsFiles();

    public IConfigurationRoot Configuration { get; }

    public T? GetService<T>() => GetServiceProvider().GetService<T>();

    public T GetRequiredService<T>()
        where T : notnull
    {
        return GetServiceProvider().GetRequiredService<T>();
    }

    public void AttachITestOutputHelper(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private IServiceProvider GetServiceProvider()
    {
        if (_serviceProvider != default)
        {
            return _serviceProvider;
        }

        if (_testOutputHelper != null)
        {
            _services.AddLogging(
                loggingBuilder =>
                    loggingBuilder.AddProvider(new XUnitLoggerProvider(_testOutputHelper))
            );
        }

        if (Configuration != null)
        {
            _services.AddSingleton<IConfiguration>(Configuration);
        }

        return _serviceProvider = _services.BuildServiceProvider();
    }

    private IConfigurationRoot GetConfigurationRoot()
    {
        var configurationFiles = GetSettingsFiles();
        var configurationBuilder = new ConfigurationBuilder().SetBasePath(
            Directory.GetCurrentDirectory()
        );

        foreach (var configurationFile in configurationFiles)
        {
            configurationBuilder.AddJsonFile(configurationFile.Trim());
        }

        return configurationBuilder.Build();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                if (_serviceProvider is not null)
                {
                    ((ServiceProvider)_serviceProvider).Dispose();
                }
                _services.Clear();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
