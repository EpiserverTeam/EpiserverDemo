using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer;
using EpiTest.Models.DynamicData;
using System.Collections.Generic;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using System.Web;

namespace EpiTest.Models.Blocks
{
    [ContentType(DisplayName = "Form Comment", GUID = "24de6cb0-5145-4043-aa71-c9e56e5db117", Description = "Nothing")]
    public class FormComment : BlockData
    {

        [CultureSpecific]
        [Display(
            Name = "Text Input",
            Description = "Name field's description",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string TextInput { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Input",
            Description = "Name field's description",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual String Input { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text Button",
            Description = "Name field's description",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        public virtual string TextButton { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Button",
            Description = "Name field's description",
            GroupName = SystemTabNames.Content,
            Order = 4)]
        public virtual Url Button { get; set; }

        public virtual List<DynamicComment> ListComment
        {
            get { return DynamicComment.GetComments(EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.IPageRouteHelper>().PageLink.ID); }
        }

        public virtual List<DynamicComment> AllComment
        {
            get { return DynamicComment.GetAllComments(); }
        }
    }
}