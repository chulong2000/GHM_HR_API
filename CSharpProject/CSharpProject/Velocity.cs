using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProject
{
    struct Velocity
    {
        public double X
        {
            readonly get;
            set;
        }

        public double Y
        {
            readonly get;
            set;
        }

        public readonly double Speed => Math.Sqrt(X * X + Y * Y);

        public readonly override string ToString() => $"({X}, {Y}) speed={Speed:F2}";
    }
}
