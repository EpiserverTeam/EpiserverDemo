using System;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EpiserverDemo.Models.Pages;
using EPiServer.ServiceLocation;

namespace EpiserverDemo.Models
{
    public class BaseClassHelper
    {
        /*
         * This is contructor of base class helper
         */
        public BaseClassHelper() { }

        /*
         * Function check or create article page
         * The first, get Root Page Name of Setting in Start Page
         * Check root page, if root page not existed, create root page
         */
        public PageData CheckOrCreateArticlePage()
        {
            var startPage = DataFactory.Instance.GetPage(PageReference.StartPage);
            string rootName = startPage["RootPageName"].ToString();

            IContentRepository contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            //PageData startPage = DataFactory.Instance.GetPage(PageReference.StartPage);
            PageData articlePage = FindPageByNameToPage(new PageReference(startPage.ContentLink.ID), rootName);
            if (articlePage == null)
            {
                return createPage(contentRepository, startPage.ContentLink.ID.ToString(), rootName);
            }
            return articlePage;
        }

        /*
         * return a page after have ID of Page Parent and Page Name
         */

        public PageData createPage(IContentRepository contentRepository, string IDofPageParent, string PageName)
        {
            PageData myPage = contentRepository.GetDefault<ContainerPage>(ContentReference.Parse(IDofPageParent));
            myPage.Name = PageName;

            contentRepository.Save(myPage, SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            return myPage;
        }

        /************************************************************************/
        /* Create A Page and no return  
         * 
         */
        /************************************************************************/

        public void createAPage(IContentRepository contentRepository, string IDofPageParent, string PageName)
        {
            PageData myPage = contentRepository.GetDefault<ContainerPage>(ContentReference.Parse(IDofPageParent));
            myPage.Name = PageName;

            contentRepository.Save(myPage, SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
             
        }

        /*
         * Find a name page children of parent page
         */

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

        /*
         * Check a Page existing Put into Name Of Page and ID Parent Page
         */

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

        /* initialization page for date time now */

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
                yearPage = FindPageByNameToPage(new PageReference(articlePage.ContentLink.ID), nameYearPage);
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

        /* Initialization pages for name Year and Article Page 
            Article Page is root page - Admin will choice root page*/

        public void initPageTreeAYear(int nameYear, PageData articlePage)
        {
            IContentRepository contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            // create year page
            PageData yearPage = FindPageByNameToPage(new PageReference(articlePage.ContentLink.ID), nameYear.ToString());
            if (yearPage != null)
            {
                return;
            }
            PageData monthPage;
            for (int i = 1; i < 13; i++)
            {
                int numberDaysOfMonth = DateTime.DaysInMonth(nameYear, i);
                monthPage = createPage(contentRepository, yearPage.ContentLink.ID.ToString(), i.ToString());
                for (int y = 1; y < numberDaysOfMonth; i++)
                {
                    createAPage(contentRepository, monthPage.ContentLink.ID.ToString(), y.ToString());
                }
            }
        }
    }
}