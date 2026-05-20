namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally applies Searchable metadata based on a feature flag.
/// When the feature is enabled, property becomes searchable based on searchableWhenEnabled.
/// When the feature is disabled, property searchability is the inverse.
/// </summary>
/// <remarks>
/// This attribute is obsolete. Use <see cref="FeatureFlaggedIndexingTypeAttribute"/> instead,
/// which provides full control over the <c>IndexingType</c> value for both the enabled and disabled states.
/// </remarks>
[Obsolete("Use FeatureFlaggedIndexingTypeAttribute instead, which supports the full IndexingType enum (Enabled, Disabled, ExcludeFromSearch). This attribute will be removed in a future version.")]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedSearchableAttribute(string featureName, bool searchableWhenEnabled = true) : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool SearchableWhenEnabled { get; } = searchableWhenEnabled;
}
