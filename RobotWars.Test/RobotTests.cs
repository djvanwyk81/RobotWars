using DataLayer;

namespace RobotWars.Test
{
    [TestClass]
    public sealed class RobotTests
    {
        [TestMethod]
        public void TestMove()
        {
            Arena testArena = new Arena(10, 10);
            Robot testRobot = new Robot("Test", System.Drawing.Color.Red, RobotFaction.Enemy, 1, 1, Directions.North);
            Collisions result = testRobot.Move(ref testArena);
            Assert.AreEqual(Collisions.None, result);
        }

        [TestMethod]
        public void TestTurnLeft()
        {
            Arena testArena = new Arena(10, 10);
            Robot testRobot = new Robot("Test", System.Drawing.Color.Red, RobotFaction.Enemy, 1, 1, Directions.North);
            testRobot.Turn(TurnDirections.Left);
            Assert.AreEqual(Directions.West, testRobot.Direction);

            testRobot.Turn(TurnDirections.Left);
            Assert.AreEqual(Directions.South, testRobot.Direction);

            testRobot.Turn(TurnDirections.Left);
            Assert.AreEqual(Directions.East, testRobot.Direction);

            testRobot.Turn(TurnDirections.Left);
            Assert.AreEqual(Directions.North, testRobot.Direction);
        }

        [TestMethod]
        public void TestTurnRight()
        {
            Arena testArena = new Arena(10, 10);
            Robot testRobot = new Robot("Test", System.Drawing.Color.Red, RobotFaction.Enemy, 1, 1, Directions.North);
            testRobot.Turn(TurnDirections.Right);
            Assert.AreEqual(Directions.East, testRobot.Direction);

            testRobot.Turn(TurnDirections.Right);
            Assert.AreEqual(Directions.South, testRobot.Direction);

            testRobot.Turn(TurnDirections.Right);
            Assert.AreEqual(Directions.West, testRobot.Direction);

            testRobot.Turn(TurnDirections.Right);
            Assert.AreEqual(Directions.North, testRobot.Direction);
        }
    }
}
