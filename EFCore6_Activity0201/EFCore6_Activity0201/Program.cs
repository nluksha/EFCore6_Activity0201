// See https://aka.ms/new-console-template for more information
using EFCore6_Activity0201.DBLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration; 
DbContextOptionsBuilder<AdventureWorksContext> optionsBuilder;

BuildConfiguration();
BuildOptions();
// ListPeople();

// ListPeopleThenOrderAndTake();
// QueryPeopleOrderedToListAndTake();

Console.WriteLine("Please enter the name:");
var res = Console.ReadLine();

int pageSize = 10;
for (int pageNumber = 0; pageNumber < 10; pageNumber++)
{
    Console.WriteLine($"Page {pageNumber + 1}");
    FiltredAndPagedResult(res, pageNumber, pageSize);
}



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