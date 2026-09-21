using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    public class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public double Value { get; set; }

        public Person person { get; set; }
    }
}
