using Microsoft.EntityFrameworkCore;
using ConsolePresentationalLayer.Controllers;
using DataLayer;

namespace ConsolePresentationalLayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AutoStoreDbContext> builder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AutoStoreDbContext>();
            builder.UseSqlServer(Connection.ConnectionString);

            AutoStoreDbContext autoStoreDbContext = new AutoStoreDbContext(builder.Options);
            VehiclesController vehiclesController = new VehiclesController(autoStoreDbContext);

            var vehicles = vehiclesController.GetAll();
            while(!vehicles.IsCompleted)
            {
                //waiting...
            }
            if(vehicles.IsCompletedSuccessfully)
            {
                foreach(var vehicle in vehicles.Result)
                {
                    Console.WriteLine($"{vehicle.Id} -> {vehicle.Manifacturer} {vehicle.Model}, {vehicle.Year}, {vehicle.PriceLv:f2} lv.");
                }
            }
        }
    }
}
