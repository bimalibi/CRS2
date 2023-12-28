using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using YSJU.ClientRegistrationSystem.AppEntities.ClientDetails;
using YSJU.ClientRegistrationSystem.AppEntities.ProductCategories;
using YSJU.ClientRegistrationSystem.AppEntities.Products;
using YSJU.ClientRegistrationSystem.Dtos.ProductManagementDtos;
using YSJU.ClientRegistrationSystem.Dtos.ResponseDtos;
using YSJU.ClientRegistrationSystem.Interfaces.ProductManagement;

namespace YSJU.ClientRegistrationSystem.AppServices.ProductManagement
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<ProductCategory, Guid> productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<ResponseDto<ProductResponseDto>> CreateProductAsync(CreateProductDto input)
        {
            try
            {
                Logger.LogInformation($"CreateProductAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"CreateProductAsync requested for User: {(CurrentUser.Id, input)}");


                var productCategoryQuery = await _productCategoryRepository.GetQueryableAsync();
                var productQuery = await _productRepository.GetQueryableAsync();

                var productCategory = productCategoryQuery.Where(x => x.Id == input.ProductCategoryId).FirstOrDefault()
                    ?? throw new UserFriendlyException("Product category not found");

                var newProduct = new Product
                {
                    Name = input.Name,
                    Manufacturer = input.Manufacturer,
                    Model = input.Model,
                    Price = input.Price,
                    ProductCategoryId = input.ProductCategoryId
                };

                await _productRepository.InsertAsync(newProduct, true);

                var result = new ProductResponseDto
                {
                    Name = newProduct.Name,
                    Manufacturer = newProduct.Manufacturer,
                    Model = newProduct.Model,
                    Price = newProduct.Price,
                    ProductCategoryId = newProduct.ProductCategoryId,
                    ProductCategoryName = productCategory.DisplayName,
                    Id = newProduct.Id
                };

                var response = new ResponseDto<ProductResponseDto>
                {
                    Success = true,
                    Code = 200,
                    Message = "New product added successfully",
                    Data = result
                };

                Logger.LogInformation($"CreateProductAsync responded for User: {CurrentUser.Id}");

                return response;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(CreateProductAsync));
                throw;
            }
        }

        public async Task<ProductResponseDto> GetProductById(Guid productId)
        {
            try
            {
                Logger.LogInformation($"GetProductById requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"GetProductById requested for User: {(CurrentUser.Id, productId)}");

                var peoductCategoryQuery = await _productCategoryRepository.GetQueryableAsync();
                var productQuery = await _productRepository.GetQueryableAsync();

                var query = (from product in productQuery
                             join productCategory in peoductCategoryQuery on product.ProductCategoryId equals productCategory.Id into productCategoryLeft
                             from productCategory in productCategoryLeft.DefaultIfEmpty()
                             where product.Id == productId
                             select new ProductResponseDto
                             {
                                 Id = product.Id,
                                 Name = product.Name,
                                 Manufacturer = product.Manufacturer,
                                 Model = product.Model,
                                 Price = product.Price,
                                 ProductCategoryId = product.ProductCategoryId,
                                 ProductCategoryName = productCategory.DisplayName
                             }).FirstOrDefault();

                Logger.LogInformation($"GetProductById responded for User: {CurrentUser.Id}");

                return query;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(GetProductById));
                throw;
            }
        }

        public async Task<ResponseDto<ProductResponseDto>> UpdateProductAsync(Guid id, UpdateProductDto input)
        {
            try
            {
                Logger.LogInformation($"UpdateProductAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"UpdateProductAsync requested for User: {(CurrentUser.Id, id)}");

                var peoductCategoryQuery = await _productCategoryRepository.GetQueryableAsync();
                var productQuery = await _productRepository.GetQueryableAsync();

                var productData = productQuery.Where(x => x.Id == id).FirstOrDefault()
                    ?? throw new UserFriendlyException("Product not found", code: "400");

                productData.Name = input.Name;
                productData.Manufacturer = input.Manufacturer;
                productData.Model = input.Model;
                productData.Price = input.Price;
                productData.ProductCategoryId = input.ProductCategoryId;

                await _productRepository.UpdateAsync(productData, true);

                var query = (from product in productQuery
                             join productCategory in peoductCategoryQuery on product.ProductCategoryId equals productCategory.Id into productCategoryLeft
                             from productCategory in productCategoryLeft.DefaultIfEmpty()
                             where product.Id == id
                             select new ProductResponseDto
                             {
                                 Id = product.Id,
                                 Name = product.Name,
                                 Manufacturer = product.Manufacturer,
                                 Model = product.Model,
                                 Price = product.Price,
                                 ProductCategoryId = product.ProductCategoryId,
                                 ProductCategoryName = productCategory.DisplayName
                             }).FirstOrDefault();

                var response = new ResponseDto<ProductResponseDto>
                {
                    Success = true,
                    Code = 200,
                    Message = "Product updated successfully",
                    Data = query
                };

                Logger.LogInformation($"UpdateProductAsync responded for User: {CurrentUser.Id}");

                return response;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(UpdateProductAsync));
                throw;
            }
        }

        public async Task<PagedResultDto<ProductResponseDto>> GetPagedAndSortedProductListAsync(PagedAndSortedProductDto input)
        {
            try
            {
                Logger.LogInformation($"GetPagedAndSortedProductListAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"GetPagedAndSortedProductListAsync requested for User: {(CurrentUser.Id, input)}");
                var peoductCategoryQuery = await _productCategoryRepository.GetQueryableAsync();
                var productQuery = await _productRepository.GetQueryableAsync();

                if (input.Sorting.IsNullOrWhiteSpace())
                {
                    input.Sorting = "CreationTime";
                }

                input.Sorting = $"{input.Sorting} {input.SortOrder}";

                if (!string.IsNullOrWhiteSpace(input.SearchKeyword))
                {
                    productQuery = productQuery.Where(x =>
                        x.Name.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.Manufacturer.ToLower().Contains(input.SearchKeyword.ToLower()) ||
                        x.Model.ToLower().Contains(input.SearchKeyword.ToLower()));
                }

                if (input.ProductCategoryId != null)
                {
                    productQuery = productQuery.Where(x => x.ProductCategoryId == input.ProductCategoryId);
                }

                var query = (from product in productQuery
                             join productCategory in peoductCategoryQuery on product.ProductCategoryId equals productCategory.Id into productCategoryLeft
                             from productCategory in productCategoryLeft.DefaultIfEmpty()
                             select new ProductResponseDto
                             {
                                 Id = product.Id,
                                 Name = product.Name,
                                 Manufacturer = product.Manufacturer,
                                 Model = product.Model,
                                 Price = product.Price,
                                 ProductCategoryId = product.ProductCategoryId,
                                 ProductCategoryName = productCategory.DisplayName,
                                 CreationTime = product.CreationTime,
                             });

                var result = query.OrderBy(input.Sorting)
                                  .Skip(input.SkipCount)
                                  .Take(input.MaxResultCount).ToList();

                var totalCount = query.Count();
                var response = new PagedResultDto<ProductResponseDto>(totalCount, result);

                Logger.LogInformation($"GetPagedAndSortedProductListAsync responded for User: {CurrentUser.Id}");

                return response;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(GetPagedAndSortedProductListAsync));
                throw;
            }
        }

        public async Task<List<ProductCategoryResponseDto>> GetProductCategoryListAsync()
        {
            try
            {
                var peoductCategoryQuery = await _productCategoryRepository.GetQueryableAsync();

                var result = peoductCategoryQuery.Select(x => new ProductCategoryResponseDto
                {
                    ProductCategoryId = x.Id,
                    ProductCategoryName = x.DisplayName
                }).ToList();

                return result;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(GetProductCategoryListAsync));
                throw;
            }
        }

        public async Task<ResponseDto<ProductResponseDto>> DeleteProductAsync(Guid clientPersonalDetailId)
        {
            try
            {
                Logger.LogInformation($"DeleteClientDetailAsync requested by User: {CurrentUser.Id}");
                Logger.LogDebug($"DeleteClientDetailAsync requested for User: {(CurrentUser.Id, clientPersonalDetailId)}");

                var productQuery = await _productRepository.GetQueryableAsync();

                var product = productQuery.Where(x => x.Id == clientPersonalDetailId).FirstOrDefault()
                    ?? throw new UserFriendlyException("Client Personal Detail not found", code: "400");

                await _productRepository.DeleteAsync(product, true);

                Logger.LogInformation($"DeleteProductAsync responded for User: {CurrentUser.Id}");

                var reposne = new ResponseDto<ProductResponseDto>
                {
                    Success = true,
                    Code = 200,
                    Message = "Product deleted successfully",
                    Data = null
                };
                return reposne;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(DeleteProductAsync));
                throw;
            }
        }
    }
}
