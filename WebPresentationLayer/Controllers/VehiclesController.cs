using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using LogicLayer.Models;

namespace WebPresentationLayer.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly AutoStoreDbContext _context;

        public VehiclesController(AutoStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetKey(string? key)
        {
            Message message = new Message();
            message.MessageValue = key;
            if (message.MessageValue == null) message.MessageValue = string.Empty;

            return View(message);
        }

        public async Task<IActionResult> WrongKey()
        {
            return View();
        }

        public async Task<IActionResult> SetKeyEdit(int? id)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", id);
            return View(routeValueDictionary);
        }

        public async Task<IActionResult> SetKeyDelete(int? id)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", id);
            return View(routeValueDictionary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetKeyEdit(int ? id, string? vehicleKey)
        {
            vehicleKey = Request.Form["vehicleKey"];
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", id);
            routeValueDictionary.Add("key", vehicleKey);

            return RedirectToAction(nameof(Edit), routeValueDictionary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetKeyDelete(int? id, string? vehicleKey)
        {
            vehicleKey = Request.Form["vehicleKey"];
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", id);
            routeValueDictionary.Add("key", vehicleKey);

            return RedirectToAction(nameof(Delete), routeValueDictionary);
        }

        // GET
        public async Task<IActionResult> Index([Bind("Id,Manifacturer,Model,,Description,VehicleType,VehicleColor,,,SellerContactInfo")] Vehicle vehicle = null, 
            [Bind("PriceLvMin")]decimal priceLvMin = 0m, [Bind("PriceLvMax")] decimal priceLvMax = -1m,
            [Bind("YearMin")] int yearMin = 0, [Bind("YearMax")] int yearMax = -1,
            [Bind("VehicleType")] int vehicleType = -1,  [Bind("VehicleColor")] int vehicleColor = -1)
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

            return View(vehicles);
        }

        // GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Manifacturer,Model,Year,Description,VehicleType,VehicleColor,ImagesFromForm,PriceLv,SellerContactInfo")] Vehicle vehicle)
        {
            List<byte[]> images = new List<byte[]>();
            foreach (var file in vehicle.ImagesFromForm)
            {
                if (file.Length == 0) continue;

                images.Add(Utility.Utility.ConvertFormFileToByteArray(file));
            }

            vehicle.Images = new List<byte[]>(images);

            if (ModelState.IsValid 
                && vehicle.Manifacturer != null && vehicle.Manifacturer.Trim() != string.Empty
                && vehicle.Model != null && vehicle.Model.Trim() != string.Empty
                && vehicle.Description != null && vehicle.Description.Trim() != string.Empty
                && vehicle.PriceLv != 0m
                && vehicle.Year != 0
                && vehicle.SellerContactInfo != null && vehicle.SellerContactInfo.Trim() != string.Empty
                )
            {
                vehicle.Manifacturer = vehicle.Manifacturer.Trim();
                vehicle.Model = vehicle.Model.Trim();
                vehicle.Description = vehicle.Description.Trim();
                vehicle.SellerContactInfo = vehicle.SellerContactInfo.Trim();


                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("key", vehicle.SetKey());
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetKey), routeValueDictionary);
            }
            return View(vehicle);
        }

        // GET
        public async Task<IActionResult> Edit(int? id, string? key)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (key == null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("id", id);
                return RedirectToAction(nameof(SetKeyEdit), routeValueDictionary);
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            if (!vehicle.CheckKey(key) && key!="admin")
                return RedirectToAction(nameof(WrongKey));

            HttpContext.Session.SetString("key", key);

            return View(vehicle);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Manifacturer,Model,Year,Description,VehicleType,VehicleColor,ImagesFromForm,PriceLv,SellerContactInfo")] Vehicle vehicle)
        {
            List<byte[]> images = new List<byte[]>();
            foreach (var file in vehicle.ImagesFromForm)
            {
                if (file.Length == 0) continue;

                images.Add(Utility.Utility.ConvertFormFileToByteArray(file));
            }

            var dbVehicle = await _context.Vehicles.FindAsync(id);

            string manifacturer = vehicle.Manifacturer;
            string model = vehicle.Model;
            string description = vehicle.Description;
            VehicleType vehicleType = vehicle.VehicleType;
            VehicleColor vehicleColor = vehicle.VehicleColor;
            decimal priceLv = vehicle.PriceLv;
            int year = vehicle.Year;
            string sellerContactInfo = vehicle.SellerContactInfo;

            if(dbVehicle != null)
                vehicle = dbVehicle;

            if(manifacturer != null && manifacturer.Trim() != string.Empty)
                vehicle.Manifacturer = manifacturer;
            if (model != null && model.Trim() != string.Empty)
                vehicle.Model = model;
            if (description != null && description.Trim() != string.Empty)
                vehicle.Description = description;
            vehicle.VehicleType = vehicleType;
            vehicleColor = vehicle.VehicleColor;
            if(images.Count > 0)
                vehicle.Images = images;
            if (priceLv != 0m)
                vehicle.PriceLv = priceLv;
            if (year != 0)
                vehicle.Year = year;
            if (sellerContactInfo != null && sellerContactInfo.Trim() != string.Empty)
                vehicle.SellerContactInfo = sellerContactInfo;


            string key = HttpContext.Session.GetString("key");
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (!vehicle.CheckKey(key) && key != "admin")
                    return RedirectToAction(nameof(WrongKey));
                try
                {
                    vehicle.Manifacturer = vehicle.Manifacturer.Trim();
                    vehicle.Model = vehicle.Model.Trim();
                    vehicle.Description = vehicle.Description.Trim();
                    vehicle.SellerContactInfo = vehicle.SellerContactInfo.Trim();

                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    HttpContext.Session.Remove("key");
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                HttpContext.Session.Remove("key");
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET
        public async Task<IActionResult> Delete(int? id, string? key)
        {

            if (id == null)
            {
                return NotFound();
            }
            if (key == null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
                routeValueDictionary.Add("id", id);
                return RedirectToAction(nameof(SetKeyDelete), routeValueDictionary);
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            if (!vehicle.CheckKey(key) && key != "admin")
                return RedirectToAction(nameof(WrongKey));

            HttpContext.Session.SetString("key", key);

            return View(vehicle);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            string key = HttpContext.Session.GetString("key");
            if (vehicle != null)
            {
                if (!vehicle.CheckKey(key) && key != "admin")
                    return RedirectToAction(nameof(WrongKey));
                _context.Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("key");
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
