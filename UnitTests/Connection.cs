using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePresentationalLayer
{
    internal static class Connection
    {
        public static string ConnectionString { get; set; } = "Server=(localdb)\\mssqllocaldb;Database=AutoStoreDbContext-6d1d2de7-309f-4ec7-a867-b057f571a2df;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
