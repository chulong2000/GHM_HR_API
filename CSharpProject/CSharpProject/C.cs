using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    public class C : B
    {
        public sealed override void DoWork() {
            Console.WriteLine("C");
        }
    }
}
