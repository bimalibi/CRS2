using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace YSJU.ClientRegistrationSystem.AppEntities.Transactions
{
    public class SellTransaction : FullAuditedAggregateRoot<Guid>
    {
        public Guid ClientId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int SellPrice { get; set; }
    }
}
