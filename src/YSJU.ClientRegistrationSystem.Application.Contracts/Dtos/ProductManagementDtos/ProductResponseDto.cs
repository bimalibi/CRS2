using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace YSJU.ClientRegistrationSystem.Dtos.ProductManagementDtos
{
    public class ProductResponseDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set;}
        public DateTime CreationTime { get; set; }

    }
}
