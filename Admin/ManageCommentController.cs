using EPiServer.PlugIn;
using EpiTest.Models.DynamicData;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiTest.Admin
{
    [GuiPlugIn(
        Area = PlugInArea.AdminMenu,
        Url = "/ManageComment/Index",
        DisplayName = "Manage Comment Plugin")]
    [Authorize(Roles = "CmsAdmins")]

    public class ManageCommentController : Controller
    {
        public ActionResult Index(int? page)
        {
            ViewBag.Comments = DynamicComment.GetAllComments().OrderByDescending(x => x.PageID).ToPagedList(page ?? 1, 10);
            return View();
        }

        public ActionResult abc()
        {
            ViewBag.Test = "abc";
            return View();
        }
    }
}