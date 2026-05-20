namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

/// <summary>
/// Conditionally controls availability of a Visual Builder content type (Experience, Section, or Element)
/// based on a feature flag.
/// </summary>
/// <remarks>
/// <para>
/// Apply this attribute to any class that inherits from <c>ExperienceData</c>, <c>SectionData</c>,
/// or <c>ElementData</c>. When the named feature flag is disabled (or enabled, depending on
/// <see cref="VisibleWhenEnabled"/>), the type will be hidden from the Visual Builder editor palette
/// and will not be offered as a valid child of any parent type.
/// </para>
/// <para>
/// This attribute works identically to <see cref="FeatureFlaggedContentTypeAttribute"/> but is provided
/// as a distinct, self-documenting alternative for Visual Builder types.
/// Both attributes are respected by <c>FeatureFlaggedContentTypeAvailabilityService</c>.
/// </para>
/// <example>
/// Hide an Experience page type unless the "visual-builder-v2" flag is on:
/// <code>
/// [ContentType(GUID = "...")]
/// [FeatureFlaggedVisualBuilderType("visual-builder-v2")]
/// public class LandingExperience : ExperienceData { }
/// </code>
/// Control an Element that is only available when a feature is OFF:
/// <code>
/// [ContentType(GUID = "...")]
/// [FeatureFlaggedVisualBuilderType("legacy-hero", VisibleWhenEnabled = false)]
/// public class LegacyHeroElement : ElementData { }
/// </code>
/// </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class FeatureFlaggedVisualBuilderTypeAttribute(string featureFlag) : Attribute
{
    public string FeatureFlag { get; } = featureFlag ?? throw new ArgumentNullException(nameof(featureFlag));

    /// <summary>
    /// When <c>true</c> (default), the type is visible when the feature is enabled.
    /// When <c>false</c>, the type is visible when the feature is disabled.
    /// </summary>
    public bool VisibleWhenEnabled { get; set; } = true;
}
