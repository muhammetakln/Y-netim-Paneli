using Business.Services;
using Core.Abstracts.IServices;
using Core.Concrete.DTOs;
using Core.Concrete.DTOS;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService service;

        public PostsController()
        {
            service = new PostService();
        }

        // GET: Posts
        public ActionResult Index()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var posts = service.GetPostList(userId);
                return View(posts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Index Hatası: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<PostListItemDto>());
            }
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            try
            {
                var post = service.GetPostDetail(id.Value);

                if (post == null)
                {
                    return HttpNotFound();
                }

                return View(post);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Details Hatası: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewPostDto model, HttpPostedFileBase CoverImage)
        {
            // Resim yükleme - Opsiyonel
            if (CoverImage != null && CoverImage.ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                string fileExtension = Path.GetExtension(CoverImage.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CoverImageUrl",
                        "Geçersiz dosya uzantısı. İzin verilen: JPG, PNG, BMP, GIF");
                }
                else if (CoverImage.ContentLength > 2 * 1024 * 1024) // 2MB
                {
                    ModelState.AddModelError("CoverImageUrl",
                        "Dosya boyutu 2MB'dan küçük olmalıdır.");
                }
                else
                {
                    try
                    {
                        string uploadFolder = Server.MapPath("~/uploads");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string fileName = Guid.NewGuid() + fileExtension;
                        string fullPath = Path.Combine(uploadFolder, fileName);

                        CoverImage.SaveAs(fullPath);

                        // ✅ SLASH İLE!
                        model.CoverImageUrl = "/uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CoverImageUrl",
                            "Dosya kaydedilirken hata: " + ex.Message);
                    }
                }
            }
            else
            {
                // Resim yoksa null
                model.CoverImageUrl = null;
                ModelState.Remove("CoverImageUrl");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.AuthorId = User.Identity.GetUserId();
                    service.CreatePost(model);

                    TempData["SuccessMessage"] = "Gönderi başarıyla oluşturuldu!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Create Hatası: {ex.Message}");
                    ModelState.AddModelError("", "Kayıt hatası: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            try
            {
                // ✅ GetPostEdit kullanıyoruz (interface'e göre)
                var model = service.GetPostEdit(id.Value);

                if (model == null)
                {
                    return HttpNotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Edit GET Hatası: {ex.Message}");
                return HttpNotFound();
            }
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpDatePostDto model, HttpPostedFileBase CoverImage)
        {
            // Yeni resim yüklendiyse
            if (CoverImage != null && CoverImage.ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                string fileExtension = Path.GetExtension(CoverImage.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CoverImageUrl", "Geçersiz dosya uzantısı.");
                }
                else if (CoverImage.ContentLength > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("CoverImageUrl", "Dosya boyutu 2MB'dan küçük olmalıdır.");
                }
                else
                {
                    try
                    {
                        string uploadFolder = Server.MapPath("~/uploads");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string fileName = Guid.NewGuid() + fileExtension;
                        string fullPath = Path.Combine(uploadFolder, fileName);

                        CoverImage.SaveAs(fullPath);

                        // ✅ SLASH İLE!
                        model.CoverImageUrl = "/uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CoverImageUrl",
                            "Dosya yüklenemedi: " + ex.Message);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = id;
                    model.AuthorId = User.Identity.GetUserId();

                    service.UpdatePost(model);

                    TempData["SuccessMessage"] = "Gönderi başarıyla güncellendi!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Edit POST Hatası: {ex.Message}");
                    ModelState.AddModelError("", "Güncelleme hatası: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            try
            {
                var post = service.GetPostDetail(id.Value);

                if (post == null)
                {
                    return HttpNotFound();
                }

                return View(post);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Delete GET Hatası: {ex.Message}");
                return HttpNotFound();
            }
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                service.DeletePost(id, userId);

                TempData["SuccessMessage"] = "Gönderi başarıyla silindi!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Delete POST Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Silme hatası: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && service is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}