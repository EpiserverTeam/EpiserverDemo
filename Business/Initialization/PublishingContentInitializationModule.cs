using System;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Security;
using EpiserverDemo.Models.Pages;
using EpiserverDemo.Models;
 

namespace EpiserverDemo.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class PublishingContentInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            //Add initialization logic, this method is called once after CMS has been initialized
            // TODO: Create full parent here before listen publishing event.
            // + Yeear: 2017
            // + Month: 1-12
            // + Each month will have: 01-31      

            // check year page day existing
            // check or init articlepage
            // if article page existing -> return article page
            BaseClassHelper baseClassHelper = new BaseClassHelper();

            PageData articlePage = baseClassHelper.CheckOrCreateArticlePage();
            var now = DateTime.Now;
            baseClassHelper.initPageTreeAYear(now.Year, articlePage);
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.PublishedContent += ContentEvents_PublishedContent;
        }

        private void ContentEvents_PublishedContent(object sender, ContentEventArgs e)
        {
            //throw new NotImplementedException();
            // TODO:
            // 1. Get Content
            // 2. Make writable clone
            // 2b1. Get real parent link by DateTime.Now
            // 2b2. Check current published content's parentlink with 2b1
            // 3. Update parent
            // 4. Call IContentRepository.Save with SaveActions equals Publish | Force current version
            // 4b. Move (IContentRepository)

            // -- test dynamic form
            BaseClassHelper baseClassHelper = new BaseClassHelper();
            var now = DateTime.Now;
            PageData articlePage = baseClassHelper.CheckOrCreateArticlePage();
            PageData dayPage = baseClassHelper.FindPageByName(now, articlePage);

            if (e.Content.ParentLink != dayPage.ContentLink)
            {
                IContentRepository contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                contentRepository.Move(e.ContentLink, dayPage.ContentLink, AccessLevel.NoAccess, AccessLevel.NoAccess);
            }

        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }

       
    }
}