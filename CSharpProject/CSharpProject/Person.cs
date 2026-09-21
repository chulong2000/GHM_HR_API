using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpProject
{
    public class Person(string name, int age)
    {
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the age of the person.
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Other properties, methods, events...
        /// </summary>
        /// <returns></returns>

        public virtual string checkString ()
        {
            return "This is a sealed method in the Person class.";
        }

        public void print()
        {
            Console.WriteLine($"Name: {Name}, Age: {Age}");
        }

    }



}
