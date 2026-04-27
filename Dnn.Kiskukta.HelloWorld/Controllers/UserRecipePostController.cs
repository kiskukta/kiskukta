using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Components;
using Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Models;

namespace Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Controllers
{
    [DnnHandleError]
    public class UserRecipePostController : DnnController
    {
        private readonly UserRecipePostManager _postManager;

        public UserRecipePostController()
        {
            _postManager = new UserRecipePostManager();
        }

        private bool IsAdminUser()
        {
            return User != null && (User.IsSuperUser || User.IsInRole("Administrators"));
        }

        public ActionResult Index(int? productId = null)
        {
            if (IsAdminUser())
            {
                ViewBag.Message = TempData["Message"];
                var allPosts = _postManager.GetPosts(ModuleContext.ModuleId, productId, false);
                return View("Moderation", allPosts);
            }

            var posts = _postManager.GetPosts(ModuleContext.ModuleId, productId, true);
            return View(posts);
        }

        public ActionResult Submit()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToDefaultRoute();
            }

            if (IsAdminUser())
            {
                return RedirectToAction("Moderation", new { ctl = "Moderation" });
            }

            ViewBag.Message = TempData["Message"];

            return View(new UserRecipePostInfo
            {
                ModuleId = ModuleContext.ModuleId
            });
        }

        [HttpPost]
        public ActionResult Submit(UserRecipePostInfo postInfo, HttpPostedFileBase imageFile)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToDefaultRoute();
            }

            if (IsAdminUser())
            {
                return RedirectToAction("Moderation", new { ctl = "Moderation" });
            }

            try
            {
                if (string.IsNullOrWhiteSpace(postInfo.RecipeName))
                {
                    ViewBag.Message = "A recept neve kötelező.";
                    return View(postInfo);
                }

                if (string.IsNullOrWhiteSpace(postInfo.CommentText))
                {
                    ViewBag.Message = "A komment kötelező.";
                    return View(postInfo);
                }

                if (imageFile == null || imageFile.ContentLength == 0)
                {
                    ViewBag.Message = "A kép feltöltése kötelező.";
                    return View(postInfo);
                }

                var extension = Path.GetExtension(imageFile.FileName);

                if (string.IsNullOrWhiteSpace(extension))
                {
                    ViewBag.Message = "Érvénytelen fájl.";
                    return View(postInfo);
                }

                extension = extension.ToLowerInvariant();

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    ViewBag.Message = "Csak .jpg, .jpeg, .png vagy .webp fájl tölthető fel.";
                    return View(postInfo);
                }

                if (imageFile.ContentLength > 5 * 1024 * 1024)
                {
                    ViewBag.Message = "A fájl túl nagy. Maximum 5 MB lehet.";
                    return View(postInfo);
                }

                var fileName = Guid.NewGuid().ToString("N") + extension;
                var folderPath = Server.MapPath("~/Portals/" + PortalSettings.PortalId + "/KiskuktaUploads/");

                Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, fileName);
                imageFile.SaveAs(fullPath);

                postInfo.ImagePath = "/Portals/" + PortalSettings.PortalId + "/KiskuktaUploads/" + fileName;
                postInfo.ModuleId = ModuleContext.ModuleId;
                postInfo.CreatedByUserId = User.UserID;
                postInfo.CreatedByDisplayName = User.DisplayName;
                postInfo.CreatedOnDate = DateTime.Now;
                postInfo.Status = "Pending";

                _postManager.CreatePost(postInfo);

                TempData["Message"] = "Sikeres beküldés! A recept függőben van, moderációra vár.";
                return RedirectToAction("Submit", new { ctl = "Submit" });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Hiba történt: " + ex.Message;
                return View(postInfo);
            }
        }

        public ActionResult Moderation(int? productId = null)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            ViewBag.Message = TempData["Message"];
            var posts = _postManager.GetPosts(ModuleContext.ModuleId, productId, false);
            return View(posts);
        }

        [HttpPost]
        public ActionResult Approve(int postId)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            TempData["Message"] = _postManager.UpdateStatus(postId, "Approved")
                ? "A recept elfogadva."
                : "Az elfogadás nem sikerült.";

            return RedirectToAction("Moderation", new { ctl = "Moderation" });
        }

        [HttpPost]
        public ActionResult Reject(int postId)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            TempData["Message"] = _postManager.UpdateStatus(postId, "Rejected")
                ? "A recept elutasítva."
                : "Az elutasítás nem sikerült.";

            return RedirectToAction("Moderation", new { ctl = "Moderation" });
        }

        [HttpPost]
        public ActionResult Delete(int postId)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            _postManager.DeletePost(postId);
            TempData["Message"] = "A recept törölve.";

            return RedirectToAction("Moderation", new { ctl = "Moderation" });
        }
    }
}