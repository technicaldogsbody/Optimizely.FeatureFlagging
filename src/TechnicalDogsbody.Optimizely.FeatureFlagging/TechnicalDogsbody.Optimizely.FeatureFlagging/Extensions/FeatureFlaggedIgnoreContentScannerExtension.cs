namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;

public class FeatureFlaggedIgnoreContentScannerExtension(IFeatureFlagProvider featureFlagProvider) : ContentScannerExtension
{
    public override bool ShouldIgnoreProperty(ContentTypeModel contentTypeModel, PropertyInfo propertyInfo)
    {
        var featureScaffold = propertyInfo.GetCustomAttribute<FeatureFlaggedIgnoreAttribute>();

        if (featureScaffold == null)
        {
            return false;
        }

        bool isFeatureEnabled = featureFlagProvider.IsEnabled(featureScaffold.FeatureName);

        return !isFeatureEnabled;
    }
}
