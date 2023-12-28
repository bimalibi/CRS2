using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YSJU.ClientRegistrationSystem.Dtos.ProductManagementDtos
{
    public class PagedAndSortedProductDto : PagedAndSortedResultRequestDto
    {
        public string? SearchKeyword { get; set; }
        public string? SortOrder { get; set; } = "asc";
        public Guid? ProductCategoryId { get; set; }
    }
}
