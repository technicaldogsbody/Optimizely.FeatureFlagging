namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Provides feature flag evaluation.
/// </summary>
public interface IFeatureFlagProvider
{
    /// <summary>
    /// Checks if a feature is enabled.
    /// </summary>
    /// <param name="featureName">The feature name.</param>
    /// <returns>True if enabled, otherwise false.</returns>
    bool IsEnabled(string featureName);

    /// <summary>
    /// Subscribes to feature flag changes.
    /// Not all providers support this - returns null if unsupported.
    /// </summary>
    /// <param name="callback">Called when any flag changes, with the flag name as parameter.</param>
    /// <returns>Disposable subscription, or null if provider doesn't support change notifications.</returns>
    IDisposable? OnFlagChanged(Action<string> callback);
}
