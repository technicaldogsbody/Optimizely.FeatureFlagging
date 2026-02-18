namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Providers;

using Microsoft.FeatureManagement;

/// <summary>
/// Feature flag provider using Microsoft.FeatureManagement.
/// This is the default implementation.
/// </summary>
public class MicrosoftFeatureFlagProvider(IFeatureManager featureManager) : IFeatureFlagProvider
{
    /// <summary>
    /// Checks if a feature is enabled.
    /// </summary>
    /// <param name="featureName">The feature name.</param>
    /// <returns>True if enabled, otherwise false.</returns>
    public bool IsEnabled(string featureName)
    {
        return featureManager
            .IsEnabledAsync(featureName)
            .GetAwaiter()
            .GetResult();
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
