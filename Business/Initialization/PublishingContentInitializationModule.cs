using System;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Security;
using EpiserverDemo.Models.Pages;

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
            PageData articlePage = CheckOrCreateArticlePage();

            var now = DateTime.Now;

            initPageForDateTime(now, articlePage);
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.PublishedContent += ContentEvents_PublishedContent;
        }

        public PageData CheckOrCreateArticlePage()
        {
            IContentRepository contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            PageData startPage = DataFactory.Instance.GetPage(PageReference.StartPage);
            PageData articlePage = FindPageByNameToPage(new PageReference(startPage.ContentLink.ID), "ArticlePage");
            if (articlePage == null)
            {
                return createPage(contentRepository, startPage.ContentLink.ID.ToString(), "ArticlePage");
            }
            return articlePage;
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

            var now = DateTime.Now;

            PageData articlePage = CheckOrCreateArticlePage();
            PageData dayPage = FindPageByName(now, articlePage);

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

        public PageData createPage(IContentRepository contentRepository, string IDofPageParent, string PageName)
        {
            PageData myPage = contentRepository.GetDefault<ContainerPage>(ContentReference.Parse(IDofPageParent));
            myPage.Name = PageName;

            contentRepository.Save(myPage, SaveAction.Publish, AccessLevel.NoAccess);
            return myPage;
        }

        public bool checkPageExisting(string name, string IDPageParent)
        {
            ContentReference tmp = ContentReference.Parse(IDPageParent);

            var criterias = new PropertyCriteriaCollection
            {
                new PropertyCriteria()
                {
                    Name = "PageName",
                    Type = PropertyDataType.String,
                    Condition = EPiServer.Filters.CompareCondition.Equal,
                    Value = name
                }
            };

            var repository = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();

            var pages = repository.FindPagesWithCriteria(
                tmp.ToPageReference(),
                criterias);
            if (pages.Count.ToString() == "0")
                return false;
            return true;
        }

        public PageData FindPageByName(DateTime now, PageData articlePage)
        {
            // init string
            string nameYearPage = now.Year.ToString();
            string nameMonthPage = now.Month.ToString();
            string nameDayPage = now.Day.ToString();
           
            PageReference oParent = new PageReference(articlePage.ContentLink.ID);

            PageReference oYear = FindAPageOfPageParent(oParent, nameYearPage);
            PageReference oMonth = FindAPageOfPageParent(oYear, nameMonthPage);
            PageData oDay = FindPageByNameToPage(oMonth, nameDayPage);


            return oDay;
        }
        // find a page from DateTime.Now

        // fine a page from array page children.
        public PageReference FindAPageOfPageParent(PageReference oParent, string namePage)
        {
            PageDataCollection oPages;
            oPages = DataFactory.Instance.GetChildren(oParent);
            if (oPages.Count == 0)
            {
                return null;
            }
            PageData tmpPage = oPages[0];
            for (int i = 0; i < oPages.Count; i++)
            {
                tmpPage = oPages[i];
                if (tmpPage.Name == namePage)
                {
                    return new PageReference(tmpPage.ContentLink.ID);
                }
            }
            return null;
        }

        public PageData FindPageByNameToPage(PageReference oParent, string namePage)
        {
            PageDataCollection oPages;
            oPages = DataFactory.Instance.GetChildren(oParent);
            if (oPages.Count == 0) return null;            
            
            PageData tmpPage = oPages[0];
            for (int i = 0; i < oPages.Count; i++)
            {
                tmpPage = oPages[i];
                if (tmpPage.Name == namePage)
                {
                    return tmpPage;
                }

            }
            return null;
        }

        public void initPageForDateTime(DateTime now, PageData articlePage)
        {
            IContentRepository contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();      
            
            string pageIDArticlePage = articlePage.ContentLink.ID.ToString();
            string nameYearPage = now.Year.ToString();
            string nameMouthPage = now.Month.ToString();
            string nameDayPage = now.Day.ToString();
            PageData yearPage;
            PageData monthPage;
            PageData dayPage;

            if (checkPageExisting(nameYearPage, pageIDArticlePage.ToString()))
            {
                // next step -> check month
                yearPage = FindPageByNameToPage(new PageReference(182), nameYearPage);
                if (checkPageExisting(nameMouthPage, yearPage.ContentLink.ID.ToString()))
                {
                    monthPage = FindPageByNameToPage(new PageReference(yearPage.ContentLink.ID), nameMouthPage);
                    if (!checkPageExisting(nameDayPage, monthPage.ContentLink.ID.ToString()))
                    {
                        // create day page
                        dayPage = createPage(contentRepository, monthPage.ContentLink.ID.ToString(), nameDayPage);
                    }

                }
                else
                {
                    // create month, day
                    monthPage = createPage(contentRepository, yearPage.ContentLink.ID.ToString(), nameMouthPage);
                    dayPage = createPage(contentRepository, monthPage.ContentLink.ID.ToString(), nameDayPage);
                }

            }
            else
            {
                // create page year month, day
                yearPage = createPage(contentRepository, pageIDArticlePage, nameYearPage);
                monthPage = createPage(contentRepository, yearPage.ContentLink.ID.ToString(), nameMouthPage);
                dayPage = createPage(contentRepository, monthPage.ContentLink.ID.ToString(), nameDayPage);
            }

        }
    }
}