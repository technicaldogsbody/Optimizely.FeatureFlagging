namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.DataAbstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Providers;

/// <summary>
/// Extension methods for configuring feature flagging services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <param name="services">The service collection.</param>
    extension(IServiceCollection services)
    {
        [Obsolete("Use AddFeatureFlaggedContentTypes() instead. This method will be removed in a future version.")]
        public IServiceCollection AddOptimizelyFeatureFlagging() => services.AddFeatureFlaggedContentTypes();

        /// <summary>
        /// Adds feature flagging support for Optimizely CMS with Microsoft Feature Management as the default provider.
        /// Registers all content scanner extensions and configures Microsoft.FeatureManagement.
        /// </summary>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddFeatureFlaggedContentTypes()
        {
            // Register Microsoft Feature Management
            services.AddFeatureManagement();
            services.AddSingleton<IFeatureFlagProvider, MicrosoftFeatureFlagProvider>();

            return services.AddOptimizelyFeatureFlaggingCore();
        }

        /// <summary>
        /// Adds feature flagging support for Optimizely CMS content types with a custom feature flag provider.
        /// Use this overload when you want to use a different feature flag provider (e.g., LaunchDarkly, Optimizely Feature Experimentation, Flagsmith).
        /// </summary>
        /// <typeparam name="TProvider">The custom feature flag provider implementation.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <example>
        /// <code>
        /// // Register provider dependencies first
        /// services.AddSingleton&lt;ILdClient&gt;(...);
        /// 
        /// // Then register feature flagged content types with your provider
        /// services.AddFeatureFlaggedContentTypes&lt;LaunchDarklyFeatureFlagProvider&gt;();
        /// </code>
        /// </example>
        public IServiceCollection AddFeatureFlaggedContentTypes<TProvider>()
            where TProvider : class, IFeatureFlagProvider
        {
            services.AddSingleton<IFeatureFlagProvider, TProvider>();
            return services.AddOptimizelyFeatureFlaggingCore();
        }

        /// <summary>
        /// Core registration of Optimizely feature flagging extensions.
        /// Registers all content scanner extensions and the content type availability service.
        /// </summary>
        private IServiceCollection AddOptimizelyFeatureFlaggingCore()
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
