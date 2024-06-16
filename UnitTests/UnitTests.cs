using ConsolePresentationalLayer;
using LogicLayer.Models;
using Microsoft.EntityFrameworkCore;
using ConsolePresentationalLayer.Controllers;
using DataLayer;

namespace UnitTests
{
    public class Tests
    {
        //Testing web controllers would be hard :(

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void VehicleKeyTest()
        {
            //ARRANGE
            Vehicle vehicle = new Vehicle();
            //ACT
            string key = vehicle.SetKey();
            //ASSERT
            Assert.That(vehicle.CheckKey(key), "Generated key does not work!");
        }

        [Test]
        public void VehicleAdminKeyTest()
        {
            //ARRANGE
            Vehicle vehicle = new Vehicle();
            //ACT
            string key = "admin";
            //ASSERT
            Assert.That(!vehicle.CheckKey(key), "Unwanted access in model!");
        }

        [Test]
        public void EnumItemsVehicleTypesTest()
        {
            //ARRANGE
            bool flag = true;
            //ACT
            foreach (var type in EnumItems.VehicleTypes)
            {
                Vehicle vehicle = new Vehicle();
                vehicle.VehicleType = (VehicleType)type.Item1;

                if (type.Item2.CompareTo(vehicle.GetVehicleTypeString()) != 0)
                {
                    flag = false; break;
                }
                //ASSERT
                Assert.That(flag, "Vehicle types mismatched!");
            }
        }

        [Test]
        public void EnumItemsVehicleColorsTest()
        {
            //ARRANGE
            bool flag = true;
            //ACT
            foreach (var type in EnumItems.VehicleColors)
            {
                Vehicle vehicle = new Vehicle();
                vehicle.VehicleColor = (VehicleColor)type.Item1;

                if (type.Item2.CompareTo(vehicle.GetVehicleColorString()) != 0)
                {
                    flag = false; break;
                }
                //ASSERT
                Assert.That(flag, "Vehicle colors mismatched!");
            }
        }

        [Test]
        public void IModelDebugImplementationTest()
        {
            //ARRANGE
            Vehicle vehicle = new Vehicle();
            Message message = new Message();

            vehicle.Manifacturer = "TEST";
            message.MessageValue = "TEST";

            bool flag = true;
            string vehicleDebug = string.Empty;
            string messageDebug = string.Empty;
            //ACT
            try 
            {
                vehicleDebug = vehicle.SingleTextOutput();
                messageDebug = message.SingleTextOutput();
            }
            catch(Exception e) 
            {
                flag = false;
            }
            finally
            {
                //ASSERT
                Assert.That(vehicleDebug != string.Empty && messageDebug != string.Empty  && flag, "IModelDebug is not implemented correctly!");
            }
            
            
        }

        [Test]
        public void ConsoleGetAllImplementationTest()
        {
            //ARRANGE
            Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AutoStoreDbContext> builder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AutoStoreDbContext>();
            builder.UseSqlServer(Connection.ConnectionString);

            AutoStoreDbContext autoStoreDbContext = new AutoStoreDbContext(builder.Options);
            VehiclesController vehiclesController = new VehiclesController(autoStoreDbContext);

            bool flag = true;
            List<Vehicle> vehiclesList = null;
            //ACT
            try
            {
                var vehicles = vehiclesController.GetAll();
                while (!vehicles.IsCompleted)
                {
                    //waiting...
                }
                if (vehicles.IsCompletedSuccessfully)
                {
                    vehiclesList = vehicles.Result;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                //ASSERT
                Assert.That(vehiclesList != null && flag, "GetAll() does not work correctly (or db is empty)!");
            }


        }
        [Test]
        public void ConsoleGetImplementationTest()
        {
            //ARRANGE
            Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AutoStoreDbContext> builder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AutoStoreDbContext>();
            builder.UseSqlServer(Connection.ConnectionString);

            AutoStoreDbContext autoStoreDbContext = new AutoStoreDbContext(builder.Options);
            VehiclesController vehiclesController = new VehiclesController(autoStoreDbContext);

            bool flag = true;
            Vehicle vehicle = null;
            //ACT
            try
            {
                var vehicles = vehiclesController.Get(1);
                while (!vehicles.IsCompleted)
                {
                    //waiting...
                }
                if (vehicles.IsCompletedSuccessfully)
                {
                    vehicle = vehicles.Result;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception e)
            {
                flag = false;
            }
            finally
            {
                //ASSERT
                Assert.That(vehicle != null && flag, "Get() does not work correctly (or db is empty)!");
            }


        }
    }
}