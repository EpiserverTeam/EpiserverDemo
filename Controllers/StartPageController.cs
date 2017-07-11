using System.Web.Mvc;
using EpiserverDemo.Models.Pages;
using EpiserverDemo.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.DataAccess;

namespace EpiserverDemo.Controllers
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        public ActionResult Index(StartPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentPage.ContentLink)) // Check if it is the StartPage or just a page of the StartPage type.
            {
                //Connect the view models logotype property to the start page's to make it editable
                var editHints = ViewData.GetEditHints<PageViewModel<StartPage>, StartPage>();
                editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
                editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
                editHints.AddConnection(m => m.Layout.CompanyInformationPages, p => p.CompanyInformationPageLinks);
                editHints.AddConnection(m => m.Layout.NewsPages, p => p.NewsPageLinks);
                editHints.AddConnection(m => m.Layout.CustomerZonePages, p => p.CustomerZonePageLinks);
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult SetRootName()
        {
            var rootname = Request.Form["rootname"];
            var startPage = DataFactory.Instance.GetPage(PageReference.StartPage);

            //startPage["RootPageName"] = rootname;
            var cloneStartPage = startPage.CreateWritableClone();
            cloneStartPage["RootPageName"] = rootname;

            IContentRepository contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

            contentRepository.Save(cloneStartPage, SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}
