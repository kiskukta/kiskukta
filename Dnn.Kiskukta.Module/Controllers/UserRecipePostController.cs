using Dnn.Kiskukta.Dnn.Kiskukta.Module.Components;
using Dnn.Kiskukta.Dnn.Kiskukta.Module.Models;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Dnn.Kiskukta.Dnn.Kiskukta.Module.Controllers
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
            return User != null && User.IsSuperUser;
        }

        private List<ProductDropdownItem> LoadProductsSafe()
        {
            try
            {
                return _postManager.GetProducts();
            }
            catch
            {
                return new List<ProductDropdownItem>();
            }
        }

        public ActionResult Index()
        {
            ViewBag.IsAdminUser = IsAdminUser();
            ViewBag.BoxProducts = LoadProductsSafe();

            return View(_postManager.GetPosts(true));
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
                ModuleId = ModuleContext.ModuleId,
                Products = LoadProductsSafe()
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
                postInfo.Products = LoadProductsSafe();

                if (string.IsNullOrWhiteSpace(postInfo.ProductBvin))
                {
                    ViewBag.Message = "Box kiválasztása kötelező.";
                    return View("Submit", postInfo);
                }

                if (string.IsNullOrWhiteSpace(postInfo.CommentText))
                {
                    ViewBag.Message = "Megjegyzés írása kötelező.";
                    return View("Submit", postInfo);
                }

                if (imageFile == null || imageFile.ContentLength == 0)
                {
                    ViewBag.Message = "A kép feltöltése kötelező.";
                    return View("Submit", postInfo);
                }

                var extension = Path.GetExtension(imageFile.FileName);

                if (string.IsNullOrWhiteSpace(extension))
                {
                    ViewBag.Message = "Érvénytelen fájl.";
                    return View("Submit", postInfo);
                }

                extension = extension.ToLowerInvariant();

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    ViewBag.Message = "Csak .jpg, .jpeg, .png vagy .webp fájl tölthető fel.";
                    return View("Submit", postInfo);
                }

                if (imageFile.ContentLength > 5 * 1024 * 1024)
                {
                    ViewBag.Message = "A fájl túl nagy. Maximum 5 MB lehet.";
                    return View("Submit", postInfo);
                }

                var fileName = Guid.NewGuid().ToString("N") + extension;
                var folderPath = Server.MapPath("~/Portals/" + PortalSettings.PortalId + "/KiskuktaUploads/");

                Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, fileName);
                imageFile.SaveAs(fullPath);

                postInfo.ImagePath = "/Portals/" + PortalSettings.PortalId + "/KiskuktaUploads/" + fileName;
                postInfo.ModuleId = ModuleContext.ModuleId;
                postInfo.CreatedByUserId = User.UserID;
                postInfo.CreatedByDisplayName = User.Username;
                postInfo.CreatedOnDate = DateTime.Now;
                postInfo.Status = "Pending";

                _postManager.CreatePost(postInfo);

                ViewBag.Message = "Sikeres beküldés! Hozzászólásod jóváhagyásra vár.";

                return View("Submit", new UserRecipePostInfo
                {
                    ModuleId = ModuleContext.ModuleId,
                    Products = LoadProductsSafe()
                });
            }
            catch (Exception ex)
            {
                postInfo.Products = LoadProductsSafe();
                ViewBag.Message = "Hiba történt: " + ex.Message;

                return View("Submit", postInfo);
            }
        }

        public ActionResult Moderation()
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            ViewBag.Message = TempData["Message"];

            return View(_postManager.GetPosts(false));
        }

        [HttpPost]
        public ActionResult Approve(int postId, string ReturnUrl)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            var success = _postManager.UpdateStatus(postId, "Approved");

            TempData["Message"] = success
                ? "Hozzászólás megjelenítve."
                : "A megjelenítés nem sikerült.";

            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Moderation", new { ctl = "Moderation" });
        }

        [HttpPost]
        public ActionResult Reject(int postId, string ReturnUrl)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            var success = _postManager.UpdateStatus(postId, "Rejected");

            TempData["Message"] = success
                ? "Hozzászólás elrejtve."
                : "Az elrejtés nem sikerült.";

            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Moderation", new { ctl = "Moderation" });
        }

        [HttpPost]
        public ActionResult Delete(int postId, string ReturnUrl)
        {
            if (!IsAdminUser())
            {
                return RedirectToDefaultRoute();
            }

            _postManager.DeletePost(postId);

            TempData["Message"] = "A recept törölve.";

            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Moderation", new { ctl = "Moderation" });
        }
    }
}