using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    public class Employee : Person
    {
        public Employee(string name, int age) : base(name, age)
        {
        }
        
        
        public String GetTypeOfData(string data)
        {
            return "Employeee";
        }
    }
}
