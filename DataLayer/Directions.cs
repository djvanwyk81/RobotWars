using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public enum Directions
    {
        [Description("North")]
        N = 0,
        [Description("East")]
        E = 1,
        [Description("South")]
        S = 2,
        [Description("West")]
        W = 3
    }

    public enum TurnDirections
    {
        [Description("Left")]
        L,
        [Description("Right")]
        R,
    }
}