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
using EPiServer.Data.Dynamic;

namespace EpiTest.Controllers
{
    public class FormCommentController : BlockController<FormComment>
    {
        
        private DynamicComment value;
        public override ActionResult Index(FormComment currentBlock)
        {
            return PartialView(currentBlock);
        }


        // Submit comment by ajax
        [HttpPost]
        [ValidateInput(false)]
        public void SubmitComment(int id, String email, String input)
        {
            if (input == "reset")
            {
                DynamicComment.Reset();
            }
            else
            {
                value = new DynamicComment(id, email, input);
                if (email == "Admin") value.Status = "block";
                value.Save();
            }
        }

        // Change status comment (show / hidden)
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

            return Redirect(Request.UrlReferrer.ToString());
        }

        // Delete comment
        public ActionResult DeleteComment()
        {
            var idComment = Request.Form["id"];
            var idPage = Request.Form["PageID"];
            int ID = int.Parse(idComment);
            int PageId = int.Parse(idPage);
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));
            store.Delete(FindComment(ID, PageId));
            return Redirect(Request.UrlReferrer.ToString());
        }


        // Delete All Comment in all page
        public ActionResult Reset()
        {
            DynamicComment.Reset();
            return Redirect(Request.UrlReferrer.ToString());
        }


        // Find comment by ID 
        public DynamicComment FindComment(int Id, int PageId)
        {
            foreach(DynamicComment key in DynamicComment.GetComments(PageId))
            {
                if (key.IdComment == Id) return key;
            }
            return new DynamicComment();
        }
    }
}