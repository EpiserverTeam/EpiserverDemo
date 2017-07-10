using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EpiTest.Models.DynamicData;
using System.Collections.Generic;

namespace EpiTest.Models.Pages
{
    [ContentType(DisplayName = "Filter Comment", GUID = "05ba1945-b213-4a82-878d-fffc8ff298d3", Description = "Filter Comments by User")]
    public class FilterComment : PageData
    {

        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }

        //public virtual List<DynamicComment> ListComment
        //{
        //    get { return DynamicComment.GetComments(); }
        //}

    }
}