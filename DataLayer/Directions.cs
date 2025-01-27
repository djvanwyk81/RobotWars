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
        North = 0,
        [Description("East")]
        East = 1,
        [Description("South")]
        South = 2,
        [Description("West")]
        West = 3
    }

    public enum TurnDirections
    {
        [Description("Left")]
        Left,
        [Description("Right")]
        Right,
    }
}