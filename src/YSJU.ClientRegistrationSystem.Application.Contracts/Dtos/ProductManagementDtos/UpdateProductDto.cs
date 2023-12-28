using System;
using System.Collections.Generic;
using System.Text;

namespace YSJU.ClientRegistrationSystem.Dtos.ProductManagementDtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
