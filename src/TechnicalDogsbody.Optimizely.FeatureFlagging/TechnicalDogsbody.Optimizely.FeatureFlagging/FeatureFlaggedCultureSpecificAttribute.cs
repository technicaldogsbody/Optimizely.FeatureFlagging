namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

/// <summary>
/// Conditionally applies CultureSpecific metadata based on a feature flag.
/// When the feature is enabled, property becomes culture-specific based on cultureSpecificWhenEnabled.
/// When the feature is disabled, property culture-specificity is the inverse.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedCultureSpecificAttribute(string featureName, bool cultureSpecificWhenEnabled = true) : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool CultureSpecificWhenEnabled { get; } = cultureSpecificWhenEnabled;
}
