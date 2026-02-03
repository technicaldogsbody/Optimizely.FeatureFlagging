namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally applies BackingType metadata based on a feature flag.
/// When the feature is enabled, uses enabledBackingType.
/// When the feature is disabled, uses disabledBackingType.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedBackingTypeAttribute(string featureName, Type enabledBackingType, Type disabledBackingType)
    : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public Type EnabledBackingType { get; } = enabledBackingType ?? throw new ArgumentNullException(nameof(enabledBackingType));
    public Type DisabledBackingType { get; } = disabledBackingType ?? throw new ArgumentNullException(nameof(disabledBackingType));
}
