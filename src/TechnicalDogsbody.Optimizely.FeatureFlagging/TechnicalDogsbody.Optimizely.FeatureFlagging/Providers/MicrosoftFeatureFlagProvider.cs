namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

/// <summary>
/// Feature flag provider using Microsoft.FeatureManagement.
/// This is the default implementation.
/// </summary>
public class MicrosoftFeatureFlagProvider(IServiceScopeFactory serviceScopeFactory) : IFeatureFlagProvider
{
    /// <summary>
    /// Checks if a feature is enabled.
    /// </summary>
    /// <param name="featureName">The feature name.</param>
    /// <returns>True if enabled, otherwise false.</returns>
    public bool IsEnabled(string featureName)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var featureManager = scope.ServiceProvider.GetRequiredService<IFeatureManager>();
        return Task.Run(() => featureManager.IsEnabledAsync(featureName)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Microsoft.FeatureManagement does not support change notifications.
    /// </summary>
    /// <param name="callback">Callback for flag changes.</param>
    /// <returns>Always returns null.</returns>
    public IDisposable? OnFlagChanged(Action<string> callback)
    {
        // Microsoft.FeatureManagement doesn't support change notifications
        return null;
    }
}
