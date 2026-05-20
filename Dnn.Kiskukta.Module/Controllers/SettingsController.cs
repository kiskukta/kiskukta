using System.Web.Mvc;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Dnn.Kiskukta.Dnn.Kiskukta.Module.Models;

namespace Dnn.Kiskukta.Dnn.Kiskukta.Module.Controllers
{
    public class SettingsController : DnnController
    {
        [HttpGet]
        public ActionResult Settings()
        {
            var model = new Settings();

            model.ProductBvin =
                ModuleContext.Configuration.ModuleSettings["ProductBvin"] as string;

            return View(model);
        }

        [HttpPost]
        public ActionResult Settings(Settings model)
        {
            ModuleController.Instance.UpdateModuleSetting(
                ModuleContext.ModuleId,
                "ProductBvin",
                model.ProductBvin
            );

            return RedirectToDefaultRoute();
        }
    }
}