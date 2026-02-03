namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally applies Display metadata based on a feature flag.
/// When the feature is enabled, uses enabledName/enabledDescription/etc.
/// When the feature is disabled, uses disabledName/disabledDescription/etc.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedDisplayAttribute(string featureName) : Attribute
{
    private const int OrderNotSet = -1;

    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));

    public string? EnabledName { get; set; }
    public string? DisabledName { get; set; }

    public string? EnabledDescription { get; set; }
    public string? DisabledDescription { get; set; }

    public string? EnabledGroupName { get; set; }
    public string? DisabledGroupName { get; set; }

    public int EnabledOrder { get; set; } = OrderNotSet;
    public int DisabledOrder { get; set; } = OrderNotSet;

    internal bool HasEnabledOrder => EnabledOrder != OrderNotSet;
    internal bool HasDisabledOrder => DisabledOrder != OrderNotSet;
}
