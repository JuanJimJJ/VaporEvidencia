﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VaporApp.Application.Responses
{
    public class ProductsResponse
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductSku { get; set; }
        public short ProductStock { get; set; }
        public byte ProductReview { get; set; }
    }
}
