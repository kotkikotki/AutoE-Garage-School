using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LogicLayer.Models
{
    public enum VehicleType
    {
        CAR,
        MOTORCYCLE,
        TRUCK
    }

    /*
        White Cars: 23.9%
        Black Cars: 23.2%
        Gray Cars: 15.5%
        Silver Cars: 14.5%
        Red Cars: 10.3%
        Blue Cars: 9.0%
        Brown Cars: 1.4%
        Green Cars: 0.7%
        Beige Cars: 0.4%
        Orange Cars: 0.4%
    */
    public enum VehicleColor
    {
        WHITE,
        BLACK,
        GRAY,
        SILVER,
        RED,
        BLUE,
        BROWN,
        GREEN,
        BEIGE,
        ORANGE,

        OTHER
    }

    public static class EnumItems
    {
        public static List<Tuple<int, string>> VehicleTypes = new List<Tuple<int, string>>()
        {
            new Tuple<int, string>(0, "Car"),
            new Tuple<int, string>(1, "Motorcycle"),
            new Tuple<int, string>(2, "Truck")
        };

        public static List<Tuple<int, string>> VehicleColors = new List<Tuple<int, string>>()
        {
            new Tuple<int, string>(0, "White"),
            new Tuple<int, string>(1, "Black"),
            new Tuple<int, string>(2, "Gray"),
            new Tuple<int, string>(3, "Silver"),
            new Tuple<int, string>(4, "Red"),
            new Tuple<int, string>(5, "Blue"),
            new Tuple<int, string>(6, "Brown"),
            new Tuple<int, string>(7, "Green"),
            new Tuple<int, string>(8, "Beige"),
            new Tuple<int, string>(9, "Orange"),
            new Tuple<int, string>(10, "Other")
        };
    }

    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Manifacturer { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(1800, int.MaxValue)]
        public int Year { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(VehicleType))]
        public VehicleType VehicleType { get; set; }

        [Required]
        [EnumDataType(typeof(VehicleColor))]
        public VehicleColor VehicleColor { get; set; }


        [Required]
        public List<byte[]> Images { get; set; } = new List<byte[]>();

        [NotMapped]
        public List<IFormFile> ImagesFromForm { get; set; } = new List<IFormFile>();

        [Required]
        [Range(0d, 7.922816251426434E+28d)] //decimal.ToDouble(decimal.MaxValue)
        public decimal PriceLv { get; set; }


        [Required]
        [StringLength(100)]
        public string SellerContactInfo { get; set; } = string.Empty;


        public string? VehicleKey { get; private set; } = null;
        public string SetKey()
        {
            if (VehicleKey != null) return string.Empty;

            string key = Guid.NewGuid().ToString();
            VehicleKey = key;
            return key;
        }
        public bool CheckKey(string key)
        {
            return key.CompareTo(VehicleKey) == 0;
        }

        public string GetVehicleTypeString()
        {
            switch (VehicleType)
            {
                case VehicleType.CAR:
                    return "Car";
                case VehicleType.MOTORCYCLE:
                    return "Motorcycle";
                case VehicleType.TRUCK:
                    return "Truck";
            }
            return string.Empty;
        }
        public string GetVehicleColorString()
        {
            switch (VehicleColor)
            {
                case VehicleColor.WHITE:
                    return "White";
                case VehicleColor.BLACK:
                    return "Black";
                case VehicleColor.GRAY:
                    return "Gray";
                case VehicleColor.SILVER:
                    return "Silver";
                case VehicleColor.RED:
                    return "Red";
                case VehicleColor.BLUE:
                    return "Blue";
                case VehicleColor.BROWN:
                    return "Brown";
                case VehicleColor.GREEN:
                    return "Green";
                case VehicleColor.BEIGE:
                    return "Beige";
                case VehicleColor.ORANGE:
                    return "Orange";
                case VehicleColor.OTHER:
                    return "Other";
            }
            return string.Empty;
        }
    }
}
