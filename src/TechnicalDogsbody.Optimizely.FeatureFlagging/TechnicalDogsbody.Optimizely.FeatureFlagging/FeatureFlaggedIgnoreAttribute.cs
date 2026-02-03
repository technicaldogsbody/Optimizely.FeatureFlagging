namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedIgnoreAttribute(string featureName) : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
}