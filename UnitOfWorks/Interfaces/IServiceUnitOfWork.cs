using PixerAPI.Services.Interfaces;

namespace PixerAPI.UnitOfWorks.Interfaces
{
    public interface IServiceUnitOfWork
    {
        public IJwtService JwtService { get; }
        public IUserService UserService { get; }
        public IRoleService RoleService { get; }
        public IFileService FileService { get; }
        public IProductService ProductService { get; }
        public IAgrementService AgreementService { get; }
        public IInventoryService InventoryService { get; }
    }
}
