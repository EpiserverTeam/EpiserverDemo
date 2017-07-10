using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Forms.Core.Models;
using EpiserverDemo.Store;

 

namespace EpiserverDemo.Models.Blocks
{
    [ContentType(DisplayName = "CommentBlock", GUID = "d3d478af-8abf-4cda-a161-10f1076edc3d", Description = "")]
    public class CommentBlock : BlockData
    {
        public virtual List<Submission> AllComment

        {
            get { return CommentStore.GetComment(); }
        }

        public bool CompareIDPageWithComment(string idPageString, int idPage)
        {
            var contentIDString = new ContentReference(idPageString);
            var contentIDInt = new ContentReference(idPage.ToString());

            return contentIDString.CompareToIgnoreWorkID(contentIDInt);
        }

    }
}