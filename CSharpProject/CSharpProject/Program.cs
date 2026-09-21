// See https://aka.ms/new-console-template for more information
using CSharpProject;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Net.WebSockets;

Complex C = new Complex(2, 4);
Console.WriteLine("Complex number C = " + C.getRealValue() +
                                 " + i " + C.getImgValue());

// Call update method
// Pass ref of C
Complex.Update(ref C);
Console.WriteLine("After updating C");
Console.WriteLine("Complex number C = " + C.getRealValue() +
                                 " + i " + C.getImgValue());


static int MyMethodHere()
{
    return 5;
}

static int Check_ABCC(int check)
{
    return check + 5;
}

int check_123 = Check_ABCC(MyMethodHere());

Console.WriteLine(check_123);

var order = new
{
    OrderId = 1,
    Customer = new { Name = "Alice", City = "Seattle" },
    Total = 150.00m
};
Console.WriteLine($"Order {order.OrderId} for {order.Customer.Name} in {order.Customer.City}");
// Output:
// Order 1 for Alice in Seattle


//var a = new { Name = "Alice", Age = 30 };
//var b = new { Name = "Alice", Age = 30 };
//var c = new { Name = "Bob", Age = 25 };


//double? temperature = 72;

//if (temperature is double degrees)
//{
//    Console.WriteLine($"Temperature is {degrees}°F.");
//}
//else
//{
//    Console.WriteLine("Temperature is not recorded.");
//}

//int? rating = null;

//int result1 = rating.GetValueOrDefault();    // 0 (default for int)
//int result2 = rating.GetValueOrDefault(-1);  // -1 (specified fallback)

//Console.WriteLine(result1); // 0
//Console.WriteLine(result2); // -1

//rating = 5;
//int result3 = rating.GetValueOrDefault(-1);  // 5 (actual value)
//Console.WriteLine(result3); // 5


int? a = 10;
int? b = 20;
int? c = null;

int? sum = a + b;   // both non-null: result is 30
int? product = a * (c ?? 1);   // one operand is null: result is null


string? message = null;

message = "Hello, World!";

// No warning: the compiler tracks that message is now not-null.
Console.WriteLine(message.Length);

string[] values = new string[3];      // Elements are null at run time.
Console.WriteLine(values[0]?.Length ?? -1);

string[] initialized = ["a", "b", "c"]; // Collection expression initializes every slot.
Console.WriteLine(initialized[0].Length);

string[]? tags = null;

// ?[] accesses an element only when the collection is non-null
string? first = tags?[0];
Console.WriteLine(first ?? "(none)"); // (none)

tags = ["csharp", "dotnet", "nullable"];
Console.WriteLine(tags?[0]);          // csharp

// Common escape sequences inside a regular string literal.







string name = "Alex";
string day = "Monday";

// Use + to build a string from variables and literals.
string greeting = "Hello " + name + ". Today is " + day + ".";
Console.WriteLine(greeting);
// => Hello Alex. Today is Monday.

// Use += to append to an existing string.
greeting += " How are you today?";
Console.WriteLine(greeting);
// => Hello Alex. Today is Monday. How are you today?

greeting = string.Concat(greeting, " How are you today?");


var order1 = new Order(42, "Shoes");
var order2 = new Order(42, "Shoes");

Console.WriteLine(order1 == order2);               // => False
Console.WriteLine(order1.Equals(order2));


var pt1 = new Point(3, 4);
var pt2 = new Point(3, 4);

Console.WriteLine(pt1 == pt2);

var products = new List<Product>
{
    new Product { Id = 1, Category = "Electronics", Value = 15.0 },
    new Product { Id = 2, Category = "Groceries", Value = 40.0 },
    new Product { Id = 3, Category = "Garden", Value = 210.3 },
    new Product { Id = 4, Category = "Pets", Value = 2.1 },
    new Product { Id = 5, Category = "Electronics", Value = 19.95 },
    new Product { Id = 6, Category = "Pets", Value = 5.50 },
    new Product { Id = 7, Category = "Electronics", Value = 250.0 },
};

var productLookup = products.ToLookup(p => p.Category);

var electronicsTotalValue = productLookup["Electronics"].Sum(p => p.Value);

Console.WriteLine($"product lookup: {electronicsTotalValue}");


Person person1 = new("Leopold", 6);
Console.WriteLine($"person1 Name = {person1.Name} Age = {person1.Age}");

Employee employee1 = new("abcc", 7);

person1 = employee1;




A a_2 = new B();
a_2.DoWork();

A a_1 = new D();

a_1.DoWork();

B b_1 = new D();

b_1.DoWork();

C c_1 = new D();

c_1.DoWork();

D d_1 = new D();
d_1.DoWork();

string[] dateStrings = ["05/01/2018 14:57:32.8", "2018-05-01 14:57:32.8",
                      "2018-05-01T14:57:32.8375298-04:00", "5/01/2018",
                      "5/01/2018 14:57:32.80 -07:00",
                      "1 May 2018 2:57:32.8 PM", "16-05-2018 1:00:32 PM",
                      "Fri, 15 May 2018 20:10:57 GMT"];
foreach (string dateString in dateStrings)
{
    if (DateTime.TryParse(dateString, out _))
        Console.WriteLine($"'{dateString}': valid");
    else
        Console.WriteLine($"'{dateString}': invalid");
}


 
static (string, int, double) QueryCityData(string name)
{
    if (name == "New York City")
        return (name, 8175133, 468.48);

    return ("", 0, 0);
}

string city = "Raleigh";
int population = 458880;
double area = 144.8;

(city, population, area) = QueryCityData("New York City");

Console.WriteLine(city);
Console.WriteLine(population);
Console.WriteLine(area);





