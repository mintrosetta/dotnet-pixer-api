namespace PixerAPI.Services.Interfaces
{
    public interface IAgrementService
    {
        Task<bool> IsCorrectAgreement(List<int> argrementsCheck);

        Task AppendAgreementsAsync(int productId, List<int> agreements);
    }
}
