using System;
using System.Collections.Generic;
using System.Security.Claims;
using Calculator.Model;

namespace Calculator.DataAccess
{
    /// <summary>
    /// Generates some random test data for the application
    /// </summary>
    public class FilippSystemSeed
    {
        private readonly DbContextFactory<EmployeeContext> _factory;

        public FilippSystemSeed(DbContextFactory<EmployeeContext> factory)
        {
            _factory = factory;
        }

        private readonly string[] _firstNames = {
            "Finn", "Jan", "Jannik", "Jonas", "Leon", "Luca", "Lukas", "Niklas", "Tim", "Tom",
            "Anna", "Hannah", "Julia", "Lara", "Laura", "Lea", "Lena", "Lisa", "Michelle", "Sarah"
        };

        private readonly string[] _lastNames = {
            "Altenburger", "Altendorf", "Altenhofen", "Altepeter", "Behmer",
            "Behner", "Behr", "Behre", "Zipper", "Zipperer", "Zipprich",
            "Zipse", "Zipser", "Zirbel", "Weirauch", "Weirich", "Weis", "Weisbach"
        };

        private readonly string[] _phoneNumbers = {
            "+49 (0)40 689 765-0", "+49 (0)173/2088764", "0800 33 00800", "+49 89-3438014", "05423 22 37",
            "0651 9 94 00 40",
            "06831 12 06 07", "07635 82 42 74", "Tel. 07635 82 42 74", "0172 7 62 53 64", "06831 34 24"
        };

        private readonly string[] _cities = {
            "Moers", "Neuss", "Düsseldorf", "Bielefeld", "Hannover", "Berlin", "Nürnberg", "Freiburg", "Siegen",
            "Gummersbach", "Au(Sieg)", "Mettmann", "Hilden", "Mönchengladbach", "München", "Dortmund"
        };

        private readonly string[] _streets = {
            "Augustusplatz", "Einsiedelstr.", "Dominikstraße", "Dominikweg", "Petersstraße", "Takuplatz",
            "Taku-Fort-Str.", "Trotha-Haus", "Hauptstr.", "Berliner Allee", "Königs Allee"
        };

        private readonly string[] _professions = {
            "Software-Architekt", "Ingenieur", "Mauermeister", "Vertriebler", "Lager und Transportmitarbeiter",
            "Elektroniker/in - Maschinen und Antriebstechnik",
            "Technische/r Systemplaner/in", "Geomatiker/in", "Mechatroniker/in - Kältetechnik", "UX Designer"
        };

        private readonly string[] _houseExtensions = {
            string.Empty, "A", "B", "C"
        };

        /// <summary>
        /// Randomize the data
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Picks a random item from a list
        /// </summary>
        /// <param name="list">A list of <c>string</c> to parse.</param>
        /// <returns>A single item from the list.</returns>
        private string RandomPick(IReadOnlyList<string> list)
        {
            var idx = _random.Next(list.Count - 1);
            return list[idx];
        }

        private Employee CreateEmployee()
        {
            var employee = new Employee()
            {
                FirstName = RandomPick(_firstNames),
                LastName = RandomPick(_lastNames),
                PhoneNumber = RandomPick(_phoneNumbers),
                Street = RandomPick(_streets),
                City = RandomPick(_cities),
                Profession = RandomPick(_professions),
                HouseNumber = _random.Next(1, 279).ToString(),
                HouseNumberExtension = RandomPick(_houseExtensions),
                ZipCode = _random.Next(10000, 99998).ToString(),
                Salary = _random.Next(1200, 6500)
            };
            return employee;
        }

        /// <summary>
        /// Check if database exists. If not, create it and seed new data.
        /// </summary>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/></returns>
        public async System.Threading.Tasks.Task CheckAndSeedDatabaseAsync(ClaimsPrincipal user)
        {
            using (var context = _factory.CreateDbContext())
            {
                context.User = user;
                var created = await context.Database.EnsureCreatedAsync();
                if (created)
                {
                    await SeedDatabaseWithEmployeeCountOfAsync(context, 50);
                }
            }
        }

        /// <summary>
        /// Generate random <see cref="Employee"/> instances and batch insert.
        /// </summary>
        /// <param name="context">The <see cref="EmployeeContext"/> to use.</param>
        /// <param name="totalCount">The count of employees to generate.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/></returns>
        private async System.Threading.Tasks.Task SeedDatabaseWithEmployeeCountOfAsync(EmployeeContext context, int totalCount)
        {
            var count = 0;
            var currentCycle = 0;
            while (count < totalCount)
            {
                var list = new List<Employee>();
                while (currentCycle++ < 100 && count++ < totalCount)
                {
                    list.Add(CreateEmployee());
                }

                if (list.Count > 0)
                {
                    await context.Employees.AddRangeAsync(list);
                    await context.SaveChangesAsync();
                }

                currentCycle = 0;
            }
        }
    }
}
