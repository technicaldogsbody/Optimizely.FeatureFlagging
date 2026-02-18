namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;

public class FeatureFlaggedCultureSpecificContentScannerExtension(IFeatureFlagProvider featureFlagProvider) : ContentScannerExtension
{
    public override void AssignValuesToProperties(ContentTypeModel contentTypeModel)
    {
        base.AssignValuesToProperties(contentTypeModel);

        foreach (var propertyDefinition in contentTypeModel.PropertyDefinitionModels)
        {
            var propertyInfo = contentTypeModel.ModelType.GetProperty(propertyDefinition.Name);
            if (propertyInfo == null)
            {
                continue;
            }

            var featureCultureSpecific = propertyInfo.GetCustomAttribute<FeatureFlaggedCultureSpecificAttribute>();
            if (featureCultureSpecific == null)
            {
                continue;
            }

            bool isFeatureEnabled = featureFlagProvider.IsEnabled(featureCultureSpecific.FeatureName);

            bool shouldBeCultureSpecific = featureCultureSpecific.CultureSpecificWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

            propertyDefinition.CultureSpecific = shouldBeCultureSpecific;
        }
    }
}
