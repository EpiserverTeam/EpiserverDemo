using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EpiTest.Models.Blocks;
using EpiTest.Models.DynamicData;
using EPiServer.Web.PageExtensions;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace EpiTest.Controllers
{
    public class FormCommentController : BlockController<FormComment>
    {
        
        private DynamicComment value;
        public override ActionResult Index(FormComment currentBlock)
        {
            return PartialView(currentBlock);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitComment1(int page, String email, String input)
        {
            value = new DynamicComment(page, email, input);
            value.Save();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public void SubmitComment(int id, String email, String input)
        {
            if (input == "reset")
            {
                DynamicComment.Reset(id);
            }
            else
            {
                value = new DynamicComment(id, email, input);
                if (email == "Admin") value.Status = "block";
                value.Save();
            }
        }

        [HttpPost]
        public ActionResult ChangeStatusComment(object sender, EventArgs e)
        {
            String id = Request.Form["id"];
            String PageID = Request.Form["PageID"];
            int ID = int.Parse(PageID);
            foreach (DynamicComment key in DynamicComment.GetComments(ID))
            {
                if (key.IdComment == int.Parse(id))
                {
                    if (key.Status == "none")
                    {
                        key.Status = "block";
                    }
                    else
                    {
                        key.Status = "none";
                    }
                    key.Save();
                    break;
                }
            }

            //Console.WriteLine("Abc");
            //Console.WriteLine(Request.UrlReferrer);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}