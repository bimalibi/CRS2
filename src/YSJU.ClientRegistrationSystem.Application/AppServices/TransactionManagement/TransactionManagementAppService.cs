using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using YSJU.ClientRegistrationSystem.AppEntities.ClientDetails;
using YSJU.ClientRegistrationSystem.AppEntities.ProductCategories;
using YSJU.ClientRegistrationSystem.AppEntities.Products;
using YSJU.ClientRegistrationSystem.AppEntities.Transactions;
using YSJU.ClientRegistrationSystem.Dtos.ResponseDtos;
using YSJU.ClientRegistrationSystem.Dtos.TransactionManagementDtos;
using YSJU.ClientRegistrationSystem.Interfaces.TransactionManagement;

namespace YSJU.ClientRegistrationSystem.AppServices.TransactionManagement
{
    public class TransactionManagementAppService : ApplicationService, ITransactionManagementAppService
    {
        private readonly IRepository<ClientDetail, Guid> _clientPersonalDetailRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
        private readonly IRepository<SellTransaction, Guid> _sellTransactionRepository;

        public TransactionManagementAppService(
            IRepository<ClientDetail, Guid> clientPersonalDetailRepository,
            IRepository<Product, Guid> productRepository = null,
            IRepository<ProductCategory, Guid> productCategoryRepository = null)
        {
            _clientPersonalDetailRepository = clientPersonalDetailRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<ResponseDto<TransactionResponseDto>> CreateSellTransactionAsync(CreateTransactionDto input)
        {
            try
            {
                Logger.LogInformation($"CreateSellTransactionAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"CreateSellTransactionAsync requested for User: {(CurrentUser.Id, input)}");

                var newTransaction = new SellTransaction
                {
                    ClientId = input.ClientId,
                    ProductId = input.ProductIdId,
                    SellPrice = input.SellPrice,
                    Quantity = input.Quantity
                };

                await _sellTransactionRepository.InsertAsync(newTransaction);

                var response = new ResponseDto<TransactionResponseDto>
                {
                    Success = true,
                    Code = 200,
                    Message = "Transaction Completed successfully",
                    Data = null
                };

                Logger.LogInformation($"CreateSellTransactionAsync responded for User: {CurrentUser.Id}");

                return response;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(CreateSellTransactionAsync));
                throw;
            }
        }
    }
}
