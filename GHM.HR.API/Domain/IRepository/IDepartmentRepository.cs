using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentSearchViewModel>> SelectAllAsync(string tenantId, string companyId);
        Task<List<DepartmentSearchViewModel>> SelectAllDepartmentsActionAsync(string tenantId, string companyId);
        Task<List<DepartmentSearchViewModel>> SelectDepartmentByTenantIdAsync(string tenantId, string companyId, string userId);
        Task<int> InsertAsync(Department department);
        Task<int> UpdateAsync(Department department);
        Task<int> DeleteAsync(Department department);
        Task<Department> GetInfoAsync(int? id);
        Task<bool> CheckExistsByNameAsync(string tenantId, string companyId, int id, string name);
        Task<bool> CheckExistNameAsync(string tenantId, string companyId, string name);
        Task<bool> CheckExistsByTenantIdAsync(string tenantId, string companyId, int id);
        Task<int> Update_IdPath_NamePath_DepartmentPathAsync(int id);
        Task<int> Update_ChildCountAsync(int id);
        Task<int> Update_IsActiveAsync(int id, bool isActive);
        Task<List<int>> GetIdChildrenAsync(int id);
        Task<int> UpdateByDepartmentNameAsync(string tenantId, string companyId, int departmentId, string departmentName);
        Task<bool> CheckParentIdAsync(int id, int parentId);

    }
}
