namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally applies Searchable metadata based on a feature flag.
/// When the feature is enabled, property becomes searchable based on searchableWhenEnabled.
/// When the feature is disabled, property searchability is the inverse.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedSearchableAttribute(string featureName, bool searchableWhenEnabled = true) : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool SearchableWhenEnabled { get; } = searchableWhenEnabled;
}
