using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp.Application.Dtos;
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
            IRepository<ProductCategory, Guid> productCategoryRepository = null,
            IRepository<SellTransaction, Guid> sellTransactionRepository = null)
        {
            _clientPersonalDetailRepository = clientPersonalDetailRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _sellTransactionRepository = sellTransactionRepository;
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

        public async Task<PagedResultDto<TransactionResponseDto>> GetPagedAndSortedSellTrasactionListAsync(PagedAndSortedTransactionDto input)
        {
            try
            {
                var clientPersonalDetailQuery = await _clientPersonalDetailRepository.GetQueryableAsync();
                var productQuery = await _productRepository.GetQueryableAsync();
                var sellTrasactionQuery = await _sellTransactionRepository.GetQueryableAsync();
                var productCategoryQuery = await _productCategoryRepository.GetQueryableAsync();


                if (input.Sorting.IsNullOrWhiteSpace())
                {
                    input.Sorting = "CreationTime";
                }

                input.Sorting = $"{input.Sorting} {input.SortOrder}";

                var query = from transaction in sellTrasactionQuery
                            join client in clientPersonalDetailQuery on transaction.ClientId equals client.Id into clientLeft
                            from client in clientLeft.DefaultIfEmpty()
                            join product in productQuery on transaction.ProductId equals product.Id into productLeft
                            from product in productLeft.DefaultIfEmpty()
                            join productCategory in productCategoryQuery on product.ProductCategoryId equals productCategory.Id into productCategoryLeft
                            from productCategory in productCategoryLeft.DefaultIfEmpty()
                            select new TransactionResponseDto
                            {
                                ClientId = client.Id,
                                ClientName = client.FirstName + " " + client.MiddleName + " " + client.LastName,
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Quantity = transaction.Quantity,
                                SellPrice = transaction.SellPrice,
                                ProductCategoryId = productCategory.Id,
                                ProductCategoryName = productCategory.DisplayName,
                                CreationTime = transaction.CreationTime,
                            };

                if (!string.IsNullOrWhiteSpace(input.SearchKeyword))
                {
                    query = query.Where(x =>
                        x.ClientId.ToString().Contains(input.SearchKeyword.ToLower()) ||
                        x.ClientName.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.ProductName.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.ProductCategoryName.ToLower().Contains(input.SearchKeyword.ToLower()));
                }

                if (input.ProductCategoryId != null)
                {
                    query = query.Where(x => x.ProductCategoryId == input.ProductCategoryId);
                }

                if (input.ProductId != null)
                {
                    query = query.Where(x => x.ProductId == input.ProductId);
                }

                if (input.ClientIds != null)
                {
                    query = query.Where(x => x.ClientId == input.ClientIds);
                }

                var result = query.OrderBy(input.Sorting)
                                  .Skip(input.SkipCount)
                                  .Take(input.MaxResultCount).ToList();

                var totalCount = query.Count();
                var response = new PagedResultDto<TransactionResponseDto>(totalCount, result);

                return response;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(GetPagedAndSortedSellTrasactionListAsync));
                throw;
            }
        }
    }
}
