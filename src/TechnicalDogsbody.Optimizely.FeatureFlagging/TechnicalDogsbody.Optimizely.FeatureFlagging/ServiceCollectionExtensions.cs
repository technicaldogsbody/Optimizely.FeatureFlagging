namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

/// <summary>
/// Extension methods for configuring feature flagging services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds feature flagging support for Optimizely CMS properties.
    /// Registers all content scanner extensions and configures Microsoft Feature Management.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddOptimizelyFeatureFlagging(
        this IServiceCollection services)
    {
        // Register Microsoft Feature Management
        services.AddFeatureManagement();

        // Register all content scanner extensions
        services.AddSingleton<FeatureFlaggedBackingTypeContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedCultureSpecificContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedDisplayContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedIgnoreContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedScaffoldColumnContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedSearchableContentScannerExtension>();

        return services;
    }

    /// <summary>
    /// Adds feature flagging support for Optimizely CMS properties without Microsoft Feature Management configuration.
    /// Use this overload if you need to configure Feature Management separately.
    /// Registers all content scanner extensions only.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddOptimizelyFeatureFlaggingExtensions(
        this IServiceCollection services)
    {
        // Register all content scanner extensions
        services.AddSingleton<FeatureFlaggedBackingTypeContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedCultureSpecificContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedDisplayContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedIgnoreContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedScaffoldColumnContentScannerExtension>();
        services.AddSingleton<FeatureFlaggedSearchableContentScannerExtension>();

        return services;
    }
}
