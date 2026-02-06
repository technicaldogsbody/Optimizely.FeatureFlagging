namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;

public class FeatureFlaggedSearchableContentScannerExtension(IFeatureFlagProvider featureFlagProvider) : ContentScannerExtension
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

            var featureSearchable = propertyInfo.GetCustomAttribute<FeatureFlaggedSearchableAttribute>();
            if (featureSearchable == null)
            {
                continue;
            }

            bool isFeatureEnabled = featureFlagProvider.IsEnabled(featureSearchable.FeatureName);

            bool shouldBeSearchable = featureSearchable.SearchableWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

            propertyDefinition.Searchable = shouldBeSearchable;
        }
    }
}
