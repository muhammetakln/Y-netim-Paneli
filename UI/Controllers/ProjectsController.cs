using Businnes.Services;
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
    public class ProjectsController : Controller
    {
        private readonly IProjectService service;

        public ProjectsController()
        {
            service = new ProjectService();
        }

        // GET: Projects
        public ActionResult Index()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var projects = service.GetProjectList(userId);
                return View(projects);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Index Error: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<ProjectListItemDto>());
            }
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            try
            {
                var project = service.GetProjectDetail(id.Value);

                if (project == null)
                {
                    return HttpNotFound();
                }

                return View(project);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Details Error: {ex.Message}");
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            try
            {
                // Kategori dropdown için
                ViewBag.Categories = new SelectList(
                    service.GetAllCategories(),
                    "Id",
                    "Name"
                );

                return View();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Create GET Error: {ex.Message}");
                return View("Error");
            }
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // HTML content için
        public ActionResult Create(NewProjectDto model, HttpPostedFileBase CoverImage)
        {
            // Resim yükleme - Opsiyonel
            if (CoverImage != null && CoverImage.ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp" };
                string fileExtension = Path.GetExtension(CoverImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CoverImageUrl",
                        "Invalid file extension. Allowed: JPG, PNG, BMP, GIF, WEBP");
                }
                else if (CoverImage.ContentLength > 5 * 1024 * 1024) // 5MB
                {
                    ModelState.AddModelError("CoverImageUrl",
                        "File size must be less than 5MB.");
                }
                else
                {
                    try
                    {
                        string uploadFolder = Server.MapPath("~/uploads/projects");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string fileName = Guid.NewGuid() + fileExtension;
                        string fullPath = Path.Combine(uploadFolder, fileName);

                        CoverImage.SaveAs(fullPath);

                        // ✅ SLASH İLE!
                        model.CoverImageUrl = "/uploads/projects/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CoverImageUrl",
                            "Error saving file: " + ex.Message);
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
                    service.CreateProject(model);

                    TempData["SuccessMessage"] = "Project created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Create POST Error: {ex.Message}");
                    ModelState.AddModelError("", "Save error: " + ex.Message);
                }
            }

            // Hata varsa kategorileri tekrar yükle
            ViewBag.Categories = new SelectList(
                service.GetAllCategories(),
                "Id",
                "Name",
                model.CategoryId
            );

            return View(model);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            try
            {
                var model = service.GetProjectEdit(id.Value);

                if (model == null)
                {
                    return HttpNotFound();
                }

                // Kategori dropdown
                ViewBag.Categories = new SelectList(
                    service.GetAllCategories(),
                    "Id",
                    "Name",
                    model.CategoryId
                );

                return View(model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Edit GET Error: {ex.Message}");
                return HttpNotFound();
            }
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // HTML content için
        public ActionResult Edit(int id, UpdateProjectDto model, HttpPostedFileBase CoverImage)
        {
            // Yeni resim yüklendiyse
            if (CoverImage != null && CoverImage.ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp" };
                string fileExtension = Path.GetExtension(CoverImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CoverImageUrl", "Invalid file extension.");
                }
                else if (CoverImage.ContentLength > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("CoverImageUrl", "File size must be less than 5MB.");
                }
                else
                {
                    try
                    {
                        string uploadFolder = Server.MapPath("~/uploads/projects");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string fileName = Guid.NewGuid() + fileExtension;
                        string fullPath = Path.Combine(uploadFolder, fileName);

                        CoverImage.SaveAs(fullPath);

                        // ✅ SLASH İLE!
                        model.CoverImageUrl = "/uploads/projects/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("CoverImageUrl",
                            "Error uploading file: " + ex.Message);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = id;
                    service.UpdateProject(model);

                    TempData["SuccessMessage"] = "Project updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Edit POST Error: {ex.Message}");
                    ModelState.AddModelError("", "Update error: " + ex.Message);
                }
            }

            // Hata varsa kategorileri tekrar yükle
            ViewBag.Categories = new SelectList(
                service.GetAllCategories(),
                "Id",
                "Name",
                model.CategoryId
            );

            return View(model);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            try
            {
                var project = service.GetProjectDetail(id.Value);

                if (project == null)
                {
                    return HttpNotFound();
                }

                return View(project);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Delete GET Error: {ex.Message}");
                return HttpNotFound();
            }
        }

        // POST: Projects/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                service.DeleteProject(id, userId);

                TempData["SuccessMessage"] = "Project deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Delete POST Error: {ex.Message}");
                TempData["ErrorMessage"] = "Delete error: " + ex.Message;
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