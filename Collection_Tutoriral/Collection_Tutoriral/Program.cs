

string[] sprintPlan = ["design", "code", "test"];

sprintPlan[0] = "chulong";



List<string> backlog = ["design", "code"];
backlog.Add("C#");

backlog.Add("test");

Console.WriteLine($"Array: {string.Join(", ", sprintPlan)}");
Console.WriteLine($"List count: {backlog.Count}");
// This example produces the following output:
//
//    Array: design, code, test
//    List count: 3
//    Priority for docs: 2
//

//List<string> workItems = ["design", "code", "test"];

//workItems.Add("review");
//workItems.Remove("code");

//Console.WriteLine(string.Join(", ", workItems));
//Console.WriteLine($"Has review: {workItems.Contains("review")}");
//Console.WriteLine($"Index of verify: {workItems.IndexOf("verify")}");

//

// Creates and initializes a new integer array and a new Object array.
int[] myIntArray = new int[5] { 1, 2, 3, 4, 5 };
Object[] myObjArray = new Object[5] { 26, 27, 28, 29, 30 };

// Prints the initial values of both arrays.
Console.WriteLine("Initially,");
Console.Write("integer array:");
PrintValues_2(myIntArray);
Console.Write("Object array: ");
PrintValues(myObjArray);


Dictionary<string, string> openWith =
    new Dictionary<string, string>();

// Add some elements to the dictionary. There are no
// duplicate keys, but some of the values are duplicates.
openWith.Add("txt", "notepad.exe");
openWith.Add("bmp", "paint.exe");
openWith.Add("dib", "paint.exe");
openWith.Add("rtf", "wordpad.exe");
openWith["doc"] = "winword.exe";

Console.WriteLine(openWith.ToString());  



static void PrintValues(Object[] myArr)
{
    foreach (Object i in myArr)
    {
        Console.Write("\t{0}", i);
    }
    Console.WriteLine();
}

static void PrintValues_2(int[] myArr)
{
    foreach (int i in myArr)
    {
        Console.Write("\t{0}", i);
    }
    Console.WriteLine();
}