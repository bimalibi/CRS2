using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YSJU.ClientRegistrationSystem.Dtos.TransactionManagementDtos
{
    public class PagedAndSortedTransactionDto : PagedAndSortedResultRequestDto
    {
        public string? SearchKeyword { get; set; }
        public string? SortOrder { get; set; } = "asc";
        public Guid? ProductCategoryId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ClientIds { get; set; }
    }
}
