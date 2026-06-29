namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;

public class FeatureFlaggedScaffoldColumnContentScannerExtension(IFeatureFlagProvider featureFlagProvider) : ContentScannerExtension
{
    public override bool ShouldIgnoreProperty(ContentTypeModel contentTypeModel, PropertyInfo propertyInfo)
    {
        var featureScaffold = propertyInfo.GetCustomAttribute<FeatureFlaggedScaffoldColumnAttribute>();

        if (featureScaffold == null)
        {
            return false;
        }

        bool isFeatureEnabled = featureFlagProvider.IsEnabled(featureScaffold.FeatureName);

        // When ScaffoldWhenEnabled is true, the property should be scaffolded when feature is enabled
        // When ScaffoldWhenEnabled is false, the property should be scaffolded when feature is disabled
        bool shouldScaffold = featureScaffold.ScaffoldWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

        // Return true to ignore (hide) the property if we shouldn't scaffold it
        return !shouldScaffold;
    }
}
