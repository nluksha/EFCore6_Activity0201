// See https://aka.ms/new-console-template for more information
using EFCore6_Activity0201.DBLibrary;
using EFCore6_Activity0201.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration; 
DbContextOptionsBuilder<AdventureWorksContext> optionsBuilder;

BuildConfiguration();
BuildOptions();
// ListPeople();

// ListPeopleThenOrderAndTake();
// QueryPeopleOrderedToListAndTake();

/*
Console.WriteLine("Please enter the name:");
var res = Console.ReadLine();

int pageSize = 10;
for (int pageNumber = 0; pageNumber < 10; pageNumber++)
{
    Console.WriteLine($"Page {pageNumber + 1}");
    FiltredAndPagedResult(res, pageNumber, pageSize);
}
*/

// ListAllSalespeople();
// ShowAllSalespeopleUsingProjection();
// GenerateSalesReportData();
GenerateSalesReportDataToDTO();
GenerateSalesReportDataToDTO();
GenerateSalesReportDataToDTO();

void BuildConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    configuration = builder.Build();
}

void BuildOptions()
{
    optionsBuilder = new DbContextOptionsBuilder<AdventureWorksContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("AdventureWorks"));
}

void ListPeople()
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var people = db.People.OrderByDescending(x => x.LastName)
            .Take(20)
            .ToList();

        foreach (var person in people)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName}");
        }
    }
}

void ListPeopleThenOrderAndTake()
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var people = db.People.ToList().OrderByDescending(x => x.LastName);

        foreach (var person in people.Take(10))
        {
            Console.WriteLine($"{person.FirstName} {person.LastName}");
        }
    }
}

void QueryPeopleOrderedToListAndTake()
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var people = db.People.OrderByDescending(x => x.LastName);
        var res = people.Take(10);

        foreach (var person in res)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName}");
        }
    }
}

void FilteredPeople(string filter)
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var searchTerm = filter.ToLower();
        var query = db.People.Where(x =>
            x.LastName.ToLower().Contains(searchTerm)
            || x.FirstName.ToLower().Contains(searchTerm)
            || x.PersonType.ToLower().Equals(searchTerm)
        );

        foreach (var person in query)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName} {person.PersonType}");
        }
    }
}

void FiltredAndPagedResult(string filter, int pageNumber, int pageSize)
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var searchTerm = filter.ToLower();
        var query = db.People.Where(x =>
            x.LastName.ToLower().Contains(searchTerm)
            || x.FirstName.ToLower().Contains(searchTerm)
            || x.PersonType.ToLower().Equals(searchTerm)
        )
            .OrderBy( x => x.LastName)
            .Skip(pageNumber * pageSize)
            .Take(pageSize);

        foreach (var person in query)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName} {person.PersonType}");
        }
    }
}

void ListAllSalespeople()
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var salespeople = db.SalesPeople
            .Include(x => x.BusinessEntity)
            .ThenInclude(y => y.BusinessEntity)
            .AsNoTracking()
            .ToList();

        foreach (var sp in salespeople)
        {
            Console.WriteLine(GetSalespersonDetail(sp));
        }
    }
}

string GetSalespersonDetail(SalesPerson sp)
{
    return $"Id: {sp.BusinessEntityId}\t|TID: {sp.TerritoryId}\t| Quota: {sp.SalesQuota}\t| Bonus: {sp.Bonus}\t| YTDSales: {sp.SalesYtd}\t| Name: {sp.BusinessEntity?.BusinessEntity?.FirstName ?? ""}, {sp.BusinessEntity?.BusinessEntity?.LastName ?? ""}";
}

void ShowAllSalespeopleUsingProjection()
{
    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var salespeople = db.SalesPeople
            .AsNoTracking()
            .Select(x => new
            {
                x.BusinessEntityId,
                x.BusinessEntity.BusinessEntity.FirstName,
                x.BusinessEntity.BusinessEntity.LastName,
                x.SalesQuota,
                x.SalesYtd,
                x.SalesLastYear
            })
            .ToList();

        foreach (var sp in salespeople)
        {
            Console.WriteLine($"BID: {sp.BusinessEntityId} | Name: {sp.LastName}, {sp.FirstName} | Quota: {sp.SalesQuota} | YTD Sales: {sp.SalesYtd} | SalesLastYear: {sp.SalesLastYear}");
        }
    }
}

void GenerateSalesReportData()
{
    Console.WriteLine("What is the minimum amount of sales?");
    var input = Console.ReadLine();
    decimal filter = 0.0m;

    if (!decimal.TryParse(input, out filter))
    {
        Console.WriteLine("Bad input");
        return;
    }

    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var salesReportData = db.SalesPeople.Select(sp => new
        {
            Id = sp.BusinessEntityId,
            sp.BusinessEntity.BusinessEntity.FirstName,
            sp.BusinessEntity.BusinessEntity.LastName,
            sp.SalesYtd,
            Territories = sp.SalesTerritoryHistories.Select(y => y.Territory.Name),
            OrderCount = sp.SalesOrderHeaders.Count(),
            TotalProductsSold = sp.SalesOrderHeaders
                .SelectMany(y => y.SalesOrderDetails)
                .Sum(z => z.OrderQty)
        })
            .Where(srdata => srdata.SalesYtd > filter)
            .OrderBy(srds => srds.LastName)
            .ThenBy(srds => srds.FirstName)
            .ThenByDescending(srds => srds.SalesYtd)
            .ToList();

        foreach (var srd in salesReportData)
        {
            Console.WriteLine($"{srd.Id} | {srd.LastName}, {srd.FirstName} | {srd.SalesYtd} | {string.Join(",", srd.Territories)} | Order Count: {srd.OrderCount} | Products Sold: {srd.TotalProductsSold}");
        }
    }
}

void GenerateSalesReportDataToDTO()
{
    Console.WriteLine("What is the minimum amount of sales?");
    var input = Console.ReadLine();
    decimal filter = 0.0m;

    if (!decimal.TryParse(input, out filter))
    {
        Console.WriteLine("Bad input");
        return;
    }

    using (var db = new AdventureWorksContext(optionsBuilder.Options))
    {
        var salesReportData = db.SalesPeople.Select(sp => new SalesReportListingDto
        {
            BusinessEntityId = sp.BusinessEntityId,
            FirstName = sp.BusinessEntity.BusinessEntity.FirstName,
            LastName = sp.BusinessEntity.BusinessEntity.LastName,
            SalesYtd = sp.SalesYtd,
            Territories = sp.SalesTerritoryHistories.Select(y => y.Territory.Name),
            TotalOrders = sp.SalesOrderHeaders.Count(),
            TotalProductsSold = sp.SalesOrderHeaders
                .SelectMany(y => y.SalesOrderDetails)
                .Sum(z => z.OrderQty)
        })
            .Where(srdata => srdata.SalesYtd > filter)
            .OrderBy(srds => srds.LastName)
            .ThenBy(srds => srds.FirstName)
            .ThenByDescending(srds => srds.SalesYtd);

        foreach (var srd in salesReportData)
        {
            Console.WriteLine(srd.ToString());
        }
    }
}