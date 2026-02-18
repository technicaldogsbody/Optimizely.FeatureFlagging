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

        bool shouldScaffold = featureScaffold.ScaffoldWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

        return !shouldScaffold;
    }
}
