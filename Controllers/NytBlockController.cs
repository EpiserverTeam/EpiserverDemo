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
using EPiServer.Editor;

namespace EpiTest.Controllers
{
    public class NytBlockController : BlockController<NytBlock>
    {
        public override ActionResult Index(NytBlock currentBlock)
        {
            return PartialView(currentBlock);
        }
    }
}
