using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    public class Complex
    {
        private int real, img;

        // Parameterize Constructor
        public Complex(int r, int i)
        {
            this.real = r;
            this.img = i;
        }

        // Method to get value of real
        public int getRealValue()
        {
            return real;
        }

        // Method to get value of img
        public int getImgValue()
        {
            return img;
        }

        // Method to update value of complex 
        // object Using reference of the object
        public static void Update(ref Complex obj)
        {
            obj.real += 5;
            obj.img += 5;
        }
    }
}
