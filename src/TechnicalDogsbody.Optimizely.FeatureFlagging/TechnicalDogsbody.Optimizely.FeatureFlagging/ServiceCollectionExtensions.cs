namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.DataAbstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

/// <summary>
/// Extension methods for configuring feature flagging services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <param name="services">The service collection.</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds feature flagging support for Optimizely CMS properties.
        /// Registers all content scanner extensions and configures Microsoft Feature Management.
        /// </summary>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddOptimizelyFeatureFlagging()
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
            services.Decorate<ContentTypeAvailabilityService, FeatureFlaggedContentTypeAvailabilityService>();

            return services;
        }

        /// <summary>
        /// Adds feature flagging support for Optimizely CMS properties without Microsoft Feature Management configuration.
        /// Use this overload if you need to configure Feature Management separately.
        /// Registers all content scanner extensions only.
        /// </summary>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddOptimizelyFeatureFlaggingExtensions()
        {
            // Register all content scanner extensions
            services.AddSingleton<FeatureFlaggedBackingTypeContentScannerExtension>();
            services.AddSingleton<FeatureFlaggedCultureSpecificContentScannerExtension>();
            services.AddSingleton<FeatureFlaggedDisplayContentScannerExtension>();
            services.AddSingleton<FeatureFlaggedIgnoreContentScannerExtension>();
            services.AddSingleton<FeatureFlaggedScaffoldColumnContentScannerExtension>();
            services.AddSingleton<FeatureFlaggedSearchableContentScannerExtension>();
            services.Decorate<ContentTypeAvailabilityService, FeatureFlaggedContentTypeAvailabilityService>();

            return services;
        }
    }
}
