﻿using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data.EFCore;
using MyBulkMealsApp.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class IngredientRepository : EfCoreRepository<Ingredient, MyBulkMealsAppContext>
    {
        public IngredientRepository(MyBulkMealsAppContext context) : base(context)
        {
        }

        public async Task<List<Measurement>> GetMeasurements()
        {
            return await context.Measurement.ToListAsync();
        }

    }
}
