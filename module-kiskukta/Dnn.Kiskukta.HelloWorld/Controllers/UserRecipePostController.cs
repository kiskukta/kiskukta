using System;
using System.IO;
using System.Linq;
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

        public ActionResult Index()
        {
            var posts = _postManager.GetPosts(ModuleContext.ModuleId)
                .Where(p => !string.IsNullOrWhiteSpace(p.Status)
                         && p.Status.Trim().Equals("Approved", StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View(posts);
        }

        public ActionResult Submit()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToDefaultRoute();
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

                extension = extension.ToLower();
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

                var fileName = Guid.NewGuid() + extension;
                var folderPath = Server.MapPath("~/Portals/0/KiskuktaUploads/");
                Directory.CreateDirectory(folderPath);
                imageFile.SaveAs(Path.Combine(folderPath, fileName));

                postInfo.ImagePath = "/Portals/0/KiskuktaUploads/" + fileName;
                postInfo.ModuleId = ModuleContext.ModuleId;
                postInfo.CreatedByUserId = User.UserID;
                postInfo.CreatedOnDate = DateTime.UtcNow;
                postInfo.Status = "Pending";

                _postManager.CreatePost(postInfo);

                TempData["Message"] = "Sikeres beküldés! A poszt jóváhagyásra vár.";
                return RedirectToAction("Submit");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Hiba történt: " + ex.Message;
                return View(postInfo);
            }
        }

        public ActionResult Moderation()
        {
            if (User == null || (!User.IsSuperUser && !User.IsInRole("Administrators")))
            {
                return RedirectToDefaultRoute();
            }

            ViewBag.Message = TempData["Message"];
            var posts = _postManager.GetPosts(ModuleContext.ModuleId);
            return View(posts);
        }

        [HttpPost]
        public ActionResult Approve(int postId)
        {
            if (User == null || (!User.IsSuperUser && !User.IsInRole("Administrators")))
            {
                return RedirectToDefaultRoute();
            }

            bool success = _postManager.UpdateStatus(postId, "Approved");

            TempData["Message"] = success
                ? "A recept jóváhagyva."
                : "A jóváhagyás nem sikerült.";

            return RedirectToAction("Moderation");
        }

        [HttpPost]
        public ActionResult Reject(int postId)
        {
            if (User == null || (!User.IsSuperUser && !User.IsInRole("Administrators")))
            {
                return RedirectToDefaultRoute();
            }

            bool success = _postManager.UpdateStatus(postId, "Rejected");

            TempData["Message"] = success
                ? "A recept elutasítva."
                : "Az elutasítás nem sikerült.";

            return RedirectToAction("Moderation");
        }

        [HttpPost]
        public ActionResult Delete(int postId)
        {
            if (User == null || (!User.IsSuperUser && !User.IsInRole("Administrators")))
            {
                return RedirectToDefaultRoute();
            }

            _postManager.DeletePost(postId);
            TempData["Message"] = "A recept törölve.";

            return RedirectToAction("Moderation");
        }
    }
}