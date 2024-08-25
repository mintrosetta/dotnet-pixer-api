using Microsoft.EntityFrameworkCore;
using PixerAPI.Models;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.Services
{
    public class AgrementService : IAgrementService
    {
        private readonly IRepoUnitOfWork repoUnitOfWork;

        public AgrementService(IRepoUnitOfWork repoUnitOfWork)
        {
            this.repoUnitOfWork = repoUnitOfWork;
        }

        public async Task AppendAgreementsAsync(int productId, List<int> agreements)
        {
            List<ProductAgreement> productAgreements = new List<ProductAgreement>();

            foreach (int agreement in agreements)
            {
                productAgreements.Add(new ProductAgreement()
                {
                    ProductId = productId,
                    ArgrementId = agreement
                });
            }

            await this.repoUnitOfWork.ProductArgrementRepository.AddRange(productAgreements);
            await this.repoUnitOfWork.CompleteAsync();
        }

        public Task<List<ProductAgreement>> GetAgreementByProductIdAsync(int id)
        {
            return this.repoUnitOfWork.ProductArgrementRepository.Find(pa => pa.ProductId == id).ToListAsync();
        }

        public async Task<bool> IsCorrectAgreement(List<int> argrementsCheck)
        {
            List<Agreement> agreements = await this.GetAgreementsAsync();
            List<int> checkedArgrements = new List<int>();
            int correctCount = 0;

            // loop for argrement require check
            for (int checkIndex = 0; checkIndex < argrementsCheck.Count; checkIndex++)
            {
                // loop argrement is checked
                for (int checkedArgrementIndex = 0; checkedArgrementIndex < checkedArgrements.Count; checkedArgrementIndex++)
                {
                    // if argrement has checked
                    if (argrementsCheck[checkIndex] == checkedArgrements[checkedArgrementIndex])
                    {
                        // skip
                        continue;
                    }
                }

                // loop for argrement from database
                for (int argrementIndex = 0; argrementIndex < agreements.Count; argrementIndex++)
                {
                    if (argrementsCheck[checkIndex] == agreements[argrementIndex].Id)
                    {
                        correctCount++;
                        checkedArgrements.Add(argrementsCheck[checkIndex]);
                    }
                }
            }

            // correct count should equire argrement require check
            return (correctCount == argrementsCheck.Count);
        }

        public async Task<List<Agreement>> GetAgreementsAsync()
        {
            return await this.repoUnitOfWork.ArgrementRepository.Find((argrement) => argrement.Name != string.Empty).ToListAsync();
        }
    }
}
