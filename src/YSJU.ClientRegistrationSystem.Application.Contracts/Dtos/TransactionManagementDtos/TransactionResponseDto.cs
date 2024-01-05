using System;
using System.Collections.Generic;
using System.Text;

namespace YSJU.ClientRegistrationSystem.Dtos.TransactionManagementDtos
{
    public class TransactionResponseDto
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int SellPrice { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
