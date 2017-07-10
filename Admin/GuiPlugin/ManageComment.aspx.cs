using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;
using EPiServer.Personalization;
using EPiServer.PlugIn;
using EPiServer.Security;
using EPiServer.Util.PlugIns;
using System.Web.UI;

namespace EpiserverDemo.Admin.GuiPlugin
{
    [GuiPlugIn(DisplayName = "Manage Comment", Description = "Admin manage all comment", Area = PlugInArea.AdminMenu, Url = "~/Admin/GuiPlugin/ManageComment.aspx")]
    public partial class ManageComment : System.Web.UI.Page
    {

        // TODO: Add your Plugin Control Code here.

    }
}