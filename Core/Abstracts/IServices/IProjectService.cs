using Core.Concrete.DTOs;
using Core.Concrete.DTOS;
using System.Collections.Generic;

namespace Core.Abstracts.IServices
{
    public interface IProjectService
    {
        // List
        IEnumerable<ProjectLisItemDto> GetProjectList(string authorId);

        // Detail
        ProjectCategoryDto GetProjectDetail(int id);

        // Edit için
        UpdateProjectDto GetProjectEdit(int id);

        // CRUD
        void CreateProject(NewProjectDto newProjectDto);
        void UpdateProject(UpdateProjectDto updateProjectDto);
        void DeleteProject(int id, string authorId);

        // Helper
        IEnumerable<ProjectCategoryDto> GetAllCategories();
    }
}