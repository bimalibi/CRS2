using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using YSJU.ClientRegistrationSystem.Dtos.ProductManagementDtos;
using YSJU.ClientRegistrationSystem.Dtos.ResponseDtos;

namespace YSJU.ClientRegistrationSystem.Interfaces.ProductManagement
{
    public interface IProductAppService : IApplicationService
    {
        Task<ResponseDto<ProductResponseDto>> CreateProductAsync(CreateProductDto input);
        Task<ProductResponseDto> GetProductById(Guid productId);
        Task<ResponseDto<ProductResponseDto>> UpdateProductAsync(Guid id, UpdateProductDto input);
        Task<PagedResultDto<ProductResponseDto>> GetPagedAndSortedProductListAsync(PagedAndSortedProductDto input);
        Task<List<ProductCategoryResponseDto>> GetProductCategoryListAsync();
        Task<ResponseDto<ProductResponseDto>> DeleteProductAsync(Guid clientPersonalDetailId);
    }
}
