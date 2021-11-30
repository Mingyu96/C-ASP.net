using System;
using System.Collections.Generic;
using System.Linq;
using CA_Project.Data;
using CA_Project.Models;
using System.Threading.Tasks;

namespace CA_Project
{
    public class DBSeed
    {
        private readonly ApplicationDBContext _db;
        public DBSeed(ApplicationDBContext db)
        {
            _db = db;
        }

        public void SeedDB()
        {
            SeedProductTable();
        }

        public void Seed()
        {
            SeedProductTable();
        }

        public void SeedProductTable()
        {
            _db.Add(new Product
            {
                ProductName = ".NET Charts",
                Price = 99,
                Desc = "Brings powerful charting capabilities to your .NET applications.",
                ImageFile = "charts.png"
            });

            _db.Add(new Product
            {
                ProductName = ".NET PayPal",
                Price = 69,
                Desc = "Integrate your .NET apps with PayPal the easy way!",
                ImageFile = "paypal.png"
            });

            _db.Add(new Product
            {
                ProductName = ".NET ML",
                Price = 299,
                Desc = "Supercharged .NET machine learning libraries.",
                ImageFile = "ml.png"
            });

            _db.Add(new Product
            {
                ProductName = ".NET Analytics",
                Price = 299,
                Desc = "Perform data mining and analytics easily in .NET.",
                ImageFile = "analytics.png"
            });

            _db.Add(new Product
            {
                ProductName = ".NET Logger",
                Price = 49,
                Desc = "Logs and aggregates events easily in your.NET apps.",
                ImageFile = "logger.png"
            });

            _db.Add(new Product
            {
                ProductName = ".NET Numerics",
                Price = 199,
                Desc = "Powerful numerical methods for your .NET simulations.",
                ImageFile = "numerics.png"
            });

            _db.SaveChanges();

        }

    }
}
