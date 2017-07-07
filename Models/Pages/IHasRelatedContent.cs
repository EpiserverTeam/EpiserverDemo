using EPiServer.Core;

namespace EpiserverDemo.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
