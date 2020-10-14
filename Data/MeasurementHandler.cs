using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    public class MeasurementHandler
    {
        private readonly MyBulkMealsAppContext _context;

        public MeasurementHandler(MyBulkMealsAppContext context)
        {
            _context = context;
        }

        public async Task<Measurement> AddMeasurementType(Measurement m)
        {
            var result = await _context.Measurement.AddAsync(m);
            await _context.SaveChangesAsync();
            return _context.Measurement.FindAsync(m.Id).Result;
        }

        public bool HasMeasurements()
        {
            return _context.Measurement.Count() > 0;
        }
    }
}
