namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using Microsoft.FeatureManagement;

/// <summary>
/// Helper methods for working with feature flags synchronously.
/// </summary>
public static class FeatureManagerExtensions
{
    /// <summary>
    /// Checks if a feature is enabled synchronously.
    /// This is necessary during CMS initialization where async operations are not supported.
    /// </summary>
    /// <param name="featureManager">The feature manager.</param>
    /// <param name="featureName">The feature name.</param>
    /// <returns>True if the feature is enabled, otherwise false.</returns>
    public static bool IsEnabled(this IFeatureManager featureManager, string featureName)
    {
        return featureManager
            .IsEnabledAsync(featureName)
            .GetAwaiter()
            .GetResult();
    }
}
