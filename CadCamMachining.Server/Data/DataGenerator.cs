using CadCamMachining.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CadCamMachining.Server.Data
{
    public class DataGenerator
    {
        private readonly ApplicationDbContext _context;

        public DataGenerator(ApplicationDbContext context)
        {
            _context = context;
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            GenerateCustomersAndContacts();
            GenerateMaterials();
            GenerateStatuses();
            GenerateOrders();
            GenerateParts();
            GenerateArticles();
        }

        public void GenerateCustomersAndContacts()
        {
            _context.Customers.AddRange(new List<Customer>()
            {
                new()
                {
                    Address = "Öxnäsvägen 4",
                    Name = "CadCamMachining",
                    Contacts = new List<Contact>()
                    {
                        new () { FirstName = "Kim", Surname = "Salvesen", PhoneNumber = "073153423" },
                        new () { FirstName = "Ole", Surname = "Salvesen", PhoneNumber = "0046164633" }

                    },
                },
                new()
                {
                    Address = "Sveagatan 21",
                    Name = "Polytorch",
                    Contacts = new List<Contact>()
                    {
                        new () { FirstName = "Max", Surname = "Salvesen", PhoneNumber = "0737454553" },
                        new () { FirstName = "Steven", Surname = "Christian", PhoneNumber = "072741324" }

                    },
                },
                new()
                {
                    Address = "Kungsgatan 21",
                    Name = "SAAB AB",
                    Contacts = new List<Contact>()
                    {
                        new () { FirstName = "Laszlo", Surname = "Fischer", PhoneNumber = "073715253" },
                    },
                },
            });
            _context.SaveChanges();
        }

        public void GenerateMaterials()
        {
            _context.Materials.AddRange(new List<Material>()
            {
                new (){Name = "s355"},
                new (){Name = "x502"},
                new (){Name = "au023"},
            });
            _context.SaveChanges();
        }

        public void GenerateStatuses()
        {
            _context.OrderStatuses.AddRange(new List<OrderStatus>()
            {
                new() { Name = "Planned" },
                new() { Name = "In Progress" },
                new() { Name = "Shipped" },
                new() { Name = "Invoiced" },
            });

            _context.ArticleStatuses.AddRange(new List<ArticleStatus>()
            {
                new() { Name = "Planned" },
                new() { Name = "Designed" },
                new() { Name = "Running" },
                new() { Name = "Completed" },
            });
            _context.SaveChanges();
        }

        public void GenerateOrders()
        {
            var cadCustomer = _context.Customers.FirstOrDefault(x => x.Name.Contains("CadCam"));
            var polyCustomer = _context.Customers.FirstOrDefault(x => x.Name.Contains("Poly"));

            var order = new Order()
            {
                Contact = cadCustomer.Contacts.FirstOrDefault(),
                Customer = cadCustomer,
                Status = _context.OrderStatuses.FirstOrDefault(x => x.Name == "Planned"),
                Name = "Personal Project",
                InCharge = null,
                OrderNo = "1"
            };

            var order2 = new Order()
            {
                Contact = polyCustomer.Contacts.FirstOrDefault(),
                Customer = polyCustomer,
                Status = _context.OrderStatuses.FirstOrDefault(x => x.Name == "In Progress"),
                InCharge = null,
                Name = "CadCam Project",
                OrderNo = "2"
            };

            _context.Orders.Add(order);
            _context.Orders.Add(order2);
            _context.SaveChanges();
        }

        public void GenerateParts()
        {
            _context.Parts.AddRange(new List<Part>()
            {
                new()
                {
                    Name = "61-104 Bracket bore head 80mm bar rev",
                    Location = "H1"
                },
                new()
                {
                    Name = "61-52 Bracket bore head 25mm bar rev",
                    Location = "H1"
                },
                new()
                {
                    Name = "73-13 Bracket bore head 80mm circle",
                    Location = "H1"
                },
            });
            _context.SaveChanges();
        }

        public void GenerateArticles()
        {
            var order = _context.Orders.FirstOrDefault();
            order.Articles.Add(new Article()
            {
                Name = "Lock",
                Part = _context.Parts.FirstOrDefault(),
                Price = 5120,
                Quantity = 5,
                Status = _context.ArticleStatuses.FirstOrDefault(),
                Material = _context.Materials.FirstOrDefault()
            });
            order.Articles.Add(new Article()
            {
                Name = "Key",
                Part = _context.Parts.ToList()[1],
                Price = 5120,
                Quantity = 5,
                Status = _context.ArticleStatuses.ToList()[1],
                Material = _context.Materials.ToList()[1],
            });

            _context.SaveChanges();
        }
    }
}
