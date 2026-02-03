namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally scaffolds a property based on a feature flag.
/// Inherits from ScaffoldColumnAttribute to work with Optimizely's metadata system.
/// When the feature is disabled, the scaffold parameter determines visibility.
/// When the feature is enabled, the inverse of scaffold parameter determines visibility.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedScaffoldColumnAttribute(string featureName, bool scaffoldWhenEnabled) : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool ScaffoldWhenEnabled { get; } = scaffoldWhenEnabled;
}