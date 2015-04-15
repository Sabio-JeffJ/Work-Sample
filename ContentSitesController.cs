using Sabio.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("contentsites")]
    public class ContentSitesController : BaseController
    {
   

        [Route("edit/{uid:guid}"), HttpGet, Authorize]
        [Route()]

        public ActionResult ContentSitesDetails(Guid? uid = null)
        {
            ItemViewModel<Guid?> model = new ItemViewModel<Guid?>();
            model.Item = uid;
            return View(model);
        }
    }
}