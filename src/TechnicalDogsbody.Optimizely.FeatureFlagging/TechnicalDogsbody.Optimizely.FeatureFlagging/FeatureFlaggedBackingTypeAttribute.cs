namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally applies BackingType metadata based on a feature flag.
/// When the feature is enabled, uses enabledBackingType.
/// When the feature is disabled, uses disabledBackingType.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedBackingTypeAttribute : Attribute
{
    public string FeatureName { get; }
    public Type EnabledBackingType { get; }
    public Type DisabledBackingType { get; }

    public FeatureFlaggedBackingTypeAttribute(string featureName, Type enabledBackingType, Type disabledBackingType)
    {
        FeatureName = featureName ?? throw new ArgumentNullException(nameof(featureName));
        EnabledBackingType = enabledBackingType ?? throw new ArgumentNullException(nameof(enabledBackingType));
        DisabledBackingType = disabledBackingType ?? throw new ArgumentNullException(nameof(disabledBackingType));
    }
}
