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
        Print(index, fb);
    }
    else if (i % 5 == 0)
    {
        Print(index, buzz);
    }
    else if (i % 3 == 0)
    {
        Print(index, buzz); 
    }
    else { Print(index, $"{i}"); }
    index++;
}
///<summary>
///This method prints a formatted string
///</summary>
static void Print(int index, string text)
{
    Console.WriteLine($"{index}. {text}");
}
  
