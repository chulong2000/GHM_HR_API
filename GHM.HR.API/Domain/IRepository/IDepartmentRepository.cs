using GHM.HR.API.Domain.Models;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IDepartmentRepository
    {
        Task<int> InsertAsync(Department department);
        Task<int> UpdateAsync(Department department);
        Task<int> DeleteAsync(Department department);
        Task<Department?> GetInfoAsync(int? id);

        Task<List<int>> GetIdChildrenAsync(int id);

        Task<int> Update_ChildCountAsync(int id);

        Task<bool> CheckExistNameAsync(string companyId, string? name);

        Task<bool> CheckParentIdAsync(int id, int parentId);
    }
}
