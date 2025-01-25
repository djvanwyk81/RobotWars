using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public enum RobotFaction
    {
        Friendly = 0,
        Enemy = 1
    }

    public class Robot
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public RobotFaction Faction { get; private set; }
        public Position Position { get; private set; }
        public Directions Direction { get; private set; }
        public string DirectionIndicator
        {
            get
            {
                switch (this.Direction)
                {
                    case Directions.N: return "↑";
                    case Directions.E: return "→";
                    case Directions.S: return "↓";
                    case Directions.W: return "←";
                    default: throw new NotImplementedException();
                } // switch
            }
        }

        public delegate void MoveHandler(Robot sender);
        public event MoveHandler OnMove;

        public Robot(string name, Color color, RobotFaction faction, int x, int y, Directions direction)
        {
            this.Name = name;
            this.Color = color;
            this.Faction = faction;
            this.Position = new Position(x, y);
            this.Direction = direction;
        }

        public void Turn(TurnDirections turnDirection)
        {
            switch (this.Direction)
            {
                case Directions.N: this.Direction = (turnDirection == TurnDirections.L ? Directions.W : Directions.E); break;
                case Directions.E: this.Direction = (turnDirection == TurnDirections.L ? Directions.N : Directions.S); break;
                case Directions.S: this.Direction = (turnDirection == TurnDirections.L ? Directions.E : Directions.W); break;
                case Directions.W: this.Direction = (turnDirection == TurnDirections.L ? Directions.S : Directions.N); break;
                default: throw new NotImplementedException();
            } // switch
        }

        public Collisions Move(ref Arena arena)
        {
            #region Validation
            if (arena == null)
                throw new ArgumentNullException("arena", "Arena has not been initialized");
            #endregion

            Position newPosition = new Position(this.Position.X, this.Position.Y);

            switch (this.Direction)
            {
                case Directions.N: ++newPosition.Y; break;
                case Directions.E: --newPosition.X; break;
                case Directions.S: --newPosition.Y; break;
                case Directions.W: ++newPosition.X; break;
                default: throw new NotImplementedException();
            } // switch

            Collisions collision = arena.CheckMove(this, newPosition);
            if (collision == Collisions.None)
            {
                this.Position.X = newPosition.X;
                this.Position.Y = newPosition.Y;

                if (OnMove != null)
                    OnMove(this);
            } // if

            return collision;
        }
    }
}