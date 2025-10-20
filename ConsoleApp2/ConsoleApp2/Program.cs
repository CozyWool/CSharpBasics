using ConsoleApp2;

var mySb = new MyStringBuilder();
mySb.Append("123"); // "123"
mySb.Insert("INSERT",1); // "1INSERT23"
mySb.Remove(3,2); // "1INERT23
Console.WriteLine($"mySb = {mySb}\tLength = {mySb.Length()}");
mySb.Clear();
Console.WriteLine($"mySb after Clear() = {mySb}\tLength = {mySb.Length()}");
var tree = new SortedSet<char>();