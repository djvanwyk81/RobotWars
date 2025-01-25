using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Position : IComparable<Position>, IEquatable<Position>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Compare two positions to see if they are the same
        /// </summary>
        /// <param name="other">The position to compare with</param>
        /// <returns>0 if the positions have the same coordinates\n1 if the positions are not the same</returns>
        public int CompareTo(Position? other)
        {
            if (other == null)
                return -1;

            if ((this.X == other.X) && (this.Y == other.Y))
                return 0;
            else
                return 1;
        }

        public bool Equals(Position? other)
        {
            if (other == null)
                return false;

            return ((this.X.Equals(other.X)) && (this.Y.Equals(other.Y)));
        }
    }
}