﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VaporApp.Application.Requests.Categories
{
    public class CreateCategoriesRequest
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
    }
}
