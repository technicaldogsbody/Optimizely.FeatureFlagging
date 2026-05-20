namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

/// <summary>
/// Conditionally applies <see cref="IndexingType"/> metadata to a property based on a feature flag.
/// </summary>
/// <remarks>
/// <para>
/// When the named feature is enabled the property's <c>IndexingType</c> is set to <paramref name="enabledIndexingType"/>;
/// when it is disabled the value is set to <paramref name="disabledIndexingType"/>.
/// </para>
/// <para>
/// This attribute replaces the obsolete <see cref="FeatureFlaggedSearchableAttribute"/> and gives full control
/// over all <see cref="IndexingType"/> values, including <c>ExcludeFromSearch</c>.
/// </para>
/// <example>
/// Make a property searchable only when the "enhanced-search" feature is on:
/// <code>
/// [FeatureFlaggedIndexingType("enhanced-search",
///     enabledIndexingType: IndexingType.Searchable,
///     disabledIndexingType: IndexingType.Disabled)]
/// public virtual string Heading { get; set; }
/// </code>
/// </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedIndexingTypeAttribute(
    string featureName,
    IndexingType enabledIndexingType = IndexingType.Searchable,
    IndexingType disabledIndexingType = IndexingType.Disabled) : Attribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public IndexingType EnabledIndexingType { get; } = enabledIndexingType;
    public IndexingType DisabledIndexingType { get; } = disabledIndexingType;
}
