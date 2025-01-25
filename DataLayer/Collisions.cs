using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public enum Collisions
    {
        None = 0,
        Boundary = 1,
        Friendly = 2,
        Enemy = 3
    }
}