namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Security;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

public class FeatureFlaggedContentTypeAvailabilityService(
    ContentTypeAvailabilityService defaultService,
    IContentTypeRepository contentTypeRepository,
    IFeatureManager featureManager)
    : ContentTypeAvailabilityService
{
    public override AvailableSetting GetSetting(string contentTypeName) => defaultService.GetSetting(contentTypeName);

    public override bool IsAllowed(string parentContentTypeName, string childContentTypeName)
    {
        if (!IsFeatureEnabled(childContentTypeName))
        {
            return false;
        }

        return defaultService.IsAllowed(parentContentTypeName, childContentTypeName);
    }

    public override IList<ContentType> ListAvailable(string contentTypeName, IPrincipal user)
    {
        var availableTypes = defaultService.ListAvailable(contentTypeName, user);

        return availableTypes.Where(ct => IsFeatureEnabled(ct.Name)).ToList();
    }

    public override IList<ContentType> ListAvailable(IContent content, bool contentFolder, IPrincipal user)
    {
        var availableTypes = defaultService.ListAvailable(content, contentFolder, user);

        return availableTypes.Where(ct => IsFeatureEnabled(ct.Name)).ToList();
    }

    private bool IsFeatureEnabled(string contentTypeName)
    {
        var contentType = contentTypeRepository.Load(contentTypeName);

        if (contentType?.ModelType?.GetCustomAttributes(typeof(FeatureFlaggedContentTypeAttribute), false)
                .FirstOrDefault() is not FeatureFlaggedContentTypeAttribute attribute)
        {
            return true;
        }

        bool featureEnabled = featureManager.IsEnabled(attribute.FeatureFlag);

        return attribute.VisibleWhenEnabled ? featureEnabled : !featureEnabled;
    }
}
