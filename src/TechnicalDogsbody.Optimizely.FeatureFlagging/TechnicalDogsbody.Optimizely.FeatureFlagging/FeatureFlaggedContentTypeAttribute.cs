namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FeatureFlaggedContentTypeAttribute(string featureFlag) : Attribute
{
    public string FeatureFlag { get; } = featureFlag;
    
    /// <summary>
    /// When true, content type is visible when feature is enabled.
    /// When false, content type is visible when feature is disabled.
    /// Default is true.
    /// </summary>
    public bool VisibleWhenEnabled { get; set; } = true;
}
