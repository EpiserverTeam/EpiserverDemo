using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer;
using EPiServer.Editor;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace EpiTest.Models.Blocks
{
    [ContentType(DisplayName = "NytBlock", GUID = "8639F153-FACA-46A7-9A6E-30E38EEF9C20", Description = "This is my block")]
    public class NytBlock : BlockData
    {

        [CultureSpecific]
        [Display(
            Name = "Url Image",
            Description = "Url of Image",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual Url Link { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Description",
            Description = "Description of Image",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual XhtmlString Text { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Select Position",
            Description = "Float of Image",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        [SelectOne(SelectionFactoryType = typeof(ImageSelection))]
        public virtual string SelectLeftRight { get; set; }

    }
}

public class ImageSelection : ISelectionFactory
{
    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
    {
        return new ISelectItem[] { new SelectItem() { Text = "Left", Value = "left" }, new SelectItem() { Text = "Right", Value = "right" } };
    }
}