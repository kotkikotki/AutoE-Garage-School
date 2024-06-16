using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using LogicLayer.Models;
using System.ComponentModel;

namespace WebPresentationLayer.Controllers
{
    public class VehiclesController
    {
        private readonly AutoStoreDbContext _context;

        public VehiclesController(AutoStoreDbContext context)
        {
            _context = context;
        }


        // GET
        public async Task<List<Vehicle>> GetAll(Vehicle vehicle = null, 
            decimal priceLvMin = 0m, decimal priceLvMax = -1m,
            int yearMin = 0, int yearMax = -1,
            int vehicleType = -1,  int vehicleColor = -1)
        {
            if (vehicle == null) vehicle = new Vehicle();

            if(priceLvMin < 0) priceLvMin = 0;
            if (priceLvMax <= 0) priceLvMax = decimal.MaxValue;

            if (yearMin < 0) yearMin = 0;
            if (yearMax <= 0) yearMax = int.MaxValue;

            if (vehicle.Manifacturer == null)
                vehicle.Manifacturer = string.Empty;
            if (vehicle.Model == null)
                vehicle.Model = string.Empty;
            if (vehicle.Description == null)
                vehicle.Description = string.Empty;
            if (vehicle.SellerContactInfo == null)
                vehicle.SellerContactInfo = string.Empty;

            vehicle.Manifacturer = vehicle.Manifacturer.Trim();
            vehicle.Model = vehicle.Model.Trim();
            vehicle.Description = vehicle.Description.Trim();
            vehicle.SellerContactInfo = vehicle.SellerContactInfo.Trim();


            List<Vehicle> vehicles = await _context.Vehicles.ToListAsync();

            vehicles = vehicles.Where(x => x.PriceLv >= priceLvMin && x.PriceLv <= priceLvMax).ToList();
            vehicles = vehicles.Where(x => x.Year >= yearMin && x.Year <= yearMax).ToList();

            if (vehicleType != -1)
                vehicles = vehicles.Where(x => x.VehicleType == vehicle.VehicleType).ToList();
            if (vehicleColor != -1)
                vehicles = vehicles.Where(x => x.VehicleColor == vehicle.VehicleColor).ToList();

            if (vehicle.Manifacturer != string.Empty)
                vehicles = vehicles.Where(x => x.Manifacturer.ToLower().Contains(vehicle.Manifacturer.ToLower())).ToList();

            if (vehicle.Model != string.Empty)
                vehicles = vehicles.Where(x => x.Model.ToLower().Contains(vehicle.Model.ToLower())).ToList();

            if (vehicle.Description != string.Empty)
                vehicles = vehicles.Where(x => x.Description.ToLower().Contains(vehicle.Description.ToLower())).ToList();

            if (vehicle.SellerContactInfo != string.Empty)
                vehicles = vehicles.Where(x => x.SellerContactInfo.ToLower().Contains(vehicle.SellerContactInfo.ToLower())).ToList();

            return vehicles;
        }

        // GET
        public async Task<Vehicle?> Get(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return null;
            }

            return vehicle;
        }

        
        // POST
        public async Task Delete(int id, string key)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                if (!vehicle.CheckKey(key) && key != "admin")
                    return;
                _context.Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
