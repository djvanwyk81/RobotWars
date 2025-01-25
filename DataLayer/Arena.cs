namespace DataLayer
{
    public class Arena
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Dictionary<string, Robot> Robots { get; private set; }

        public Arena(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.Robots = new Dictionary<string, Robot>();
        }

        public bool CheckBoundaries(Position newPosition)
        {
            return ((newPosition.X < 0) || (newPosition.X > this.Width) || (newPosition.Y < 0) || (newPosition.Y > Height));
        }

        public Robot? CheckRobotCollisions(string robotName, Position newPosition)
        {
            return this.Robots.Values.Where(r => !r.Name.Equals(robotName) && r.Position.Equals(newPosition)).FirstOrDefault();
        }

        public bool CheckNames(string checkName)
        {
            return this.Robots.Keys.Where(n => n.Equals(checkName)).Count() > 0;
        }

        public Collisions CheckMove(Robot robot, Position newPosition)
        {
            #region Validation
            if (Robots == null)
                throw new NullReferenceException("Robots have not been initialized");
            #endregion

            if (CheckBoundaries(newPosition))
                return Collisions.Boundary;

            Robot? collisionRobot = this.Robots.Values.Where(r => !r.Name.Equals(robot.Name) && r.Position.Equals(robot.Position)).FirstOrDefault();

            if (collisionRobot == null)
                return Collisions.None;
            else
                return robot.Faction == collisionRobot.Faction ? Collisions.Friendly : Collisions.Enemy;
        }
    }
}