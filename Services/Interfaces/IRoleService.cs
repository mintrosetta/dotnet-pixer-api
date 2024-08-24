namespace PixerAPI.Services.Interfaces
{
    public interface IRoleService
    {
        Task<int> GetRoleIdUserAsync();
        Task<int> GetRoleIdAdminAsync();
    }
}
