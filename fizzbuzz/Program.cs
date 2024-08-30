// See https://aka.ms/new-console-template for more information


using System.Collections.Generic;

var fizz = "Fizz";
var buzz = "Buzz";
var fb = fizz + buzz;

//value represent the index 
var index = 1;

//Add a for loop to that loops to 100
for (int i = 0; i <= 100; i++)
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        Console.WriteLine($"{index}. {fb}");
    }
    else if (i % 5 == 0)
    {
        Console.WriteLine($"{index}. {buzz}");
    }
    else if (i % 3 == 0)
    {
        Console.WriteLine($"{index}. {fizz}");
    }
    else { Console.WriteLine($"{index}. {i}"); }
    index++;
}
  
