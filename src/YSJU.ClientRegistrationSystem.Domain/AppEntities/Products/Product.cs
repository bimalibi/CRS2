using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace YSJU.ClientRegistrationSystem.AppEntities.Products
{
    public class Product : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
