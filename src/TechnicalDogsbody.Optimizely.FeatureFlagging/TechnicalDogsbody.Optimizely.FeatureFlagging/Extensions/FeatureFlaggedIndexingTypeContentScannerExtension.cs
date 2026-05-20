namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;

public class FeatureFlaggedIndexingTypeContentScannerExtension(
    IFeatureFlagProvider featureFlagProvider,
    IContentTypeRepository contentTypeRepository) : ContentScannerExtension
{
    public override void AssignValuesToProperties(ContentTypeModel contentTypeModel)
    {
        base.AssignValuesToProperties(contentTypeModel);

        ContentType? writableContentType = null;
        bool hasChanges = false;

        foreach (var propertyDefinition in contentTypeModel.PropertyDefinitionModels)
        {
            var propertyInfo = contentTypeModel.ModelType.GetProperty(propertyDefinition.Name);
            if (propertyInfo == null)
            {
                continue;
            }

            var featureIndexingType = propertyInfo.GetCustomAttribute<FeatureFlaggedIndexingTypeAttribute>();
            if (featureIndexingType == null)
            {
                continue;
            }

            var existingDef = propertyDefinition.ExistingPropertyDefinition;
            if (existingDef == null)
            {
                continue;
            }

            bool isFeatureEnabled = featureFlagProvider.IsEnabled(featureIndexingType.FeatureName);
            var targetIndexingType = isFeatureEnabled
                ? featureIndexingType.EnabledIndexingType
                : featureIndexingType.DisabledIndexingType;

            if (existingDef.IndexingType == targetIndexingType)
            {
                continue;
            }

            writableContentType ??= (ContentType)contentTypeRepository.Load(existingDef.ContentTypeID).CreateWritableClone();

            var writablePropDef = writableContentType.PropertyDefinitions
                .FirstOrDefault(p => p.Name == existingDef.Name);
            if (writablePropDef == null)
            {
                continue;
            }

            writablePropDef.IndexingType = targetIndexingType;
            hasChanges = true;
        }

        if (hasChanges && writableContentType != null)
        {
            contentTypeRepository.Save(writableContentType);
        }
    }
}
