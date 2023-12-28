using System;
using System.Collections.Generic;
using System.Text;

namespace YSJU.ClientRegistrationSystem.Dtos.TransactionManagementDtos
{
    public class CreateTransactionDto
    {
        public Guid ClientId { get; set; }
        public Guid ProductIdId { get; set; }
        public int Quantity { get; set; }
        public int SellPrice { get; set; }
    }
}
