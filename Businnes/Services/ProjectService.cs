using Core.Abstracts;
using Core.Abstracts.IServices;
using Core.Concrete.DTOs;
using Core.Concrete.DTOS;
using Core.Concrete.Entities;
using Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Businnes.Services
{
    public class ProjectService : IProjectService, IDisposable
    {
        private readonly ApplicationDbContext db;

        public ProjectService()
        {
            db = new ApplicationDbContext();
        }

        // ✅ GET PROJECT LIST
        public IEnumerable<ProjectLisItemDto> GetProjectList(string authorId)
        {
            try
            {
                var projects = db.Projects
                    .Where(p => p.IsDeleted == false)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new ProjectLisItemDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Slug = p.Slug,
                        Description = p.Description,
                        Content = p.Content
                    })
                    .ToList();

                return projects;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetProjectList Error: {ex.Message}");
                return new List<ProjectLisItemDto>();
            }
        }

        // ✅ GET PROJECT DETAIL
        public ProjectCategoryDto GetProjectDetail(int id)
        {
            try
            {
                var project = db.Projects
                    .Where(p => p.Id == id && p.IsDeleted == false)
                    .FirstOrDefault();

                if (project == null) return null;

                return new ProjectCategoryDto
                {
                    Id = project.CategoryId,
                    Name = project.Category?.Name,
                    Icon = project.Category?.Icon
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetProjectDetail Error: {ex.Message}");
                return null;
            }
        }

        // ✅ GET PROJECT FOR EDIT
        public UpdateProjectDto GetProjectEdit(int id)
        {
            try
            {
                var project = db.Projects
                    .Where(p => p.Id == id && p.IsDeleted == false)
                    .FirstOrDefault();

                if (project == null) return null;

                return new UpdateProjectDto
                {
                    Id = project.Id,
                    Title = project.Title,
                    Slug = project.Slug,
                    Description = project.Description,
                    Content = project.Content,
                    CoverImageUrl = project.CoverImageUrl,
                    IsActive = project.IsActive,
                    IsFeatured = project.IsFeatured,
                    CategoryId = project.CategoryId
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetProjectEdit Error: {ex.Message}");
                return null;
            }
        }

        // ✅ CREATE PROJECT
        public void CreateProject(NewProjectDto newProjectDto)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== CREATE PROJECT ===");
                System.Diagnostics.Debug.WriteLine($"Title: {newProjectDto.Title}");
                System.Diagnostics.Debug.WriteLine($"CategoryId: {newProjectDto.CategoryId}");

                var project = new Project
                {
                    Title = newProjectDto.Title,
                    Slug = string.IsNullOrEmpty(newProjectDto.Slug)
                        ? GenerateSlug(newProjectDto.Title)
                        : newProjectDto.Slug,
                    Description = newProjectDto.Description,
                    Content = newProjectDto.Content,
                    CoverImageUrl = newProjectDto.CoverImageUrl,
                    IsActive = newProjectDto.IsActive,
                    IsFeatured = newProjectDto.IsFeatured,
                    CategoryId = newProjectDto.CategoryId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                db.Projects.Add(project);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"SUCCESS! Project ID: {project.Id}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateProject Error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner: {ex.InnerException.Message}");

                    if (ex.InnerException.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner Inner: {ex.InnerException.InnerException.Message}");
                    }
                }

                throw;
            }
        }

        // ✅ UPDATE PROJECT
        public void UpdateProject(UpdateProjectDto updateProjectDto)
        {
            try
            {
                var project = db.Projects.Find(updateProjectDto.Id);

                if (project == null || project.IsDeleted)
                {
                    throw new Exception("Project not found");
                }

                project.Title = updateProjectDto.Title;
                project.Slug = string.IsNullOrEmpty(updateProjectDto.Slug)
                    ? GenerateSlug(updateProjectDto.Title)
                    : updateProjectDto.Slug;
                project.Description = updateProjectDto.Description;
                project.Content = updateProjectDto.Content;

                // Sadece yeni resim varsa güncelle
                if (!string.IsNullOrEmpty(updateProjectDto.CoverImageUrl))
                {
                    project.CoverImageUrl = updateProjectDto.CoverImageUrl;
                }

                project.IsActive = updateProjectDto.IsActive;
                project.IsFeatured = updateProjectDto.IsFeatured;
                project.CategoryId = updateProjectDto.CategoryId;
                project.UpdatedAt = DateTime.Now;

                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"Project Updated: {project.Title}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateProject Error: {ex.Message}");
                throw;
            }
        }

        // ✅ DELETE PROJECT (Soft Delete)
        public void DeleteProject(int id, string authorId)
        {
            try
            {
                var project = db.Projects.Find(id);

                if (project == null || project.IsDeleted)
                {
                    throw new Exception("Project not found");
                }

                // Soft delete
                project.IsDeleted = true;
                project.UpdatedAt = DateTime.Now;

                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"Project Deleted: {project.Title}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteProject Error: {ex.Message}");
                throw;
            }
        }

        // ✅ GET ALL CATEGORIES
        public IEnumerable<ProjectCategoryDto> GetAllCategories()
        {
            try
            {
                var categories = db.ProjectCategories
                    .Where(c => c.IsActive && c.IsDeleted == false)
                    .OrderBy(c => c.Name)
                    .Select(c => new ProjectCategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Icon = c.Icon
                    })
                    .ToList();

                return categories;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllCategories Error: {ex.Message}");
                return new List<ProjectCategoryDto>();
            }
        }

        // ✅ HELPER: Generate Slug
        private string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            // Küçük harfe çevir
            string slug = title.ToLowerInvariant();

            // Türkçe karakterleri değiştir
            slug = slug.Replace("ı", "i")
                       .Replace("ğ", "g")
                       .Replace("ü", "u")
                       .Replace("ş", "s")
                       .Replace("ö", "o")
                       .Replace("ç", "c");

            // Özel karakterleri kaldır
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Birden fazla boşluğu tek boşluğa çevir
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", " ").Trim();

            // Boşlukları tire yap
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s", "-");

            return slug;
        }

        // ✅ DISPOSE
        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}