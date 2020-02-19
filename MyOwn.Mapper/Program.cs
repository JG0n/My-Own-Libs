using System;

namespace MyOwn.Mapper
{
    class Program
    {
        static void Main()
        {
            var person = new Person
            {
                Name = new PersonName
                {
                    FirstName = "John",
                    MiddleName = "F.",
                    LastName = "Doe"
                },
                Age = 24,
                FavoriteMovie = "Top Gun",
                Address = new Address
                {
                    Street = "My own street",
                    Street2 = "N 13",
                    City = "Tokyo",
                    Country = "Japan",
                    PostalCode = "1234"
                }
            };

            var mapper = new MyOwnMapper((cfg) =>
            {
                cfg.CreateMap<Person, PersonDto>()
                    .ForMember(destination => destination.FirstName,
                        source => source.Name.FirstName)
                    .ForMember(destination => destination.LastName,
                        source => source.Name.LastName);
            });

            var personDto = mapper.Map<PersonDto>(person);

            Print(person, personDto);
        }

        static void Print(Person person, PersonDto personDto)
        {
            Console.WriteLine($"{nameof(Person)} >> {nameof(PersonDto)}");

            PrintString(nameof(Person.Name.FirstName), person?.Name?.FirstName, personDto?.FirstName);
            PrintString(nameof(Person.Name.MiddleName),person.Name?.MiddleName, null);
            PrintString(nameof(Person.Name.LastName), person?.Name?.LastName, personDto?.LastName);

            PrintInt(nameof(Person.Age), person?.Age, personDto?.Age);
            PrintString(nameof(Person.FavoriteMovie), person?.FavoriteMovie, personDto?.FavoriteMovie);

            PrintString(nameof(Person.Address.Street), person?.Address?.Street, personDto?.Address?.Street);
            PrintString(nameof(Person.Address.Street2), person?.Address?.Street2, personDto?.Address?.Street2);
            PrintString(nameof(Person.Address.City), person?.Address?.City, personDto?.Address?.City);
            PrintString(nameof(Person.Address.Country), person?.Address?.Country, personDto?.Address?.Country);
            PrintString(nameof(Person.Address.PostalCode), person?.Address?.PostalCode, personDto?.Address?.ZipCode);
        }

        static void PrintString(string propName, string s1, string s2)
        {
            PrintProp(propName);
            Console.WriteLine($"{s1 ?? "<null>"} \t>>\t {s2 ?? "<null>"}");
        }

        static void PrintInt(string propName, int? n1, int? n2)
        {
            PrintProp(propName);
            Console.WriteLine((n1.HasValue ? n1.ToString() : "<null>") +
                " \t>>\t " + (n2.HasValue ? n2.ToString() : "<null>"));
        }

        static void PrintProp(string propName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{propName}: ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}