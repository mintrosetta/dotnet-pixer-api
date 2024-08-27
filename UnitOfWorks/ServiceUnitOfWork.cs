using PixerAPI.Services;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.UnitOfWorks
{
    public class ServiceUnitOfWork : IServiceUnitOfWork
    {
        public ServiceUnitOfWork(IRepoUnitOfWork repoUnitOfWork, IConfiguration configuration)
        {
            this.JwtService = new JwtService(configuration);
            this.UserService = new UserService(repoUnitOfWork);
            this.RoleService = new RoleService(repoUnitOfWork);
            this.AgreementService = new AgrementService(repoUnitOfWork);
            this.ProductService = new ProductService(repoUnitOfWork);
            this.InventoryService = new InventoryService(repoUnitOfWork);

            this.EmailService = new EmailServices(configuration);
            this.FileService = new FileService();
        }

        public IJwtService JwtService { get; private set; }
        public IUserService UserService { get; private set; }
        public IRoleService RoleService { get; private set; }
        public IFileService FileService { get; private set; }
        public IAgrementService AgreementService { get; private set; }
        public IProductService ProductService { get; private set; }
        public IInventoryService InventoryService { get; private set; }
        public IEmailService EmailService { get; private set; }
    }
}
