﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocceryShopping.Models.ViewModels
{
    public class ProductUpdate
    {

        public Product Product { get; set; }

        public List<Category> Categories { get; set; }
    }
}