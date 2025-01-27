using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace RobotWars.Test
{
    [TestClass]
    public sealed class ArenaTests
    {
        [TestMethod]
        public void TestBoundaries()
        {
            Arena testArena = new Arena(10, 10);
            bool result = testArena.CheckBoundaries(new Position(-1, 0));
            Assert.AreEqual(true, result);

            result = testArena.CheckBoundaries(new Position(0, -1));
            Assert.AreEqual(true, result);

            result = testArena.CheckBoundaries(new Position(11, 10));
            Assert.AreEqual(true, result);

            result = testArena.CheckBoundaries(new Position(10, 11));
            Assert.AreEqual(true, result);

            result = testArena.CheckBoundaries(new Position(5, 5));
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestNames()
        {
            Arena testArena = new Arena(10, 10);
            Robot testRobot = new Robot("Test", System.Drawing.Color.Red, RobotFaction.Enemy, 1, 1, Directions.North);
            testArena.Robots.Add(testRobot.Name, testRobot);
            
            bool result = testArena.CheckNames("Test");
            Assert.AreEqual(true, result);

            result = testArena.CheckNames("Other");
            Assert.AreEqual(false, result);
        }
    }
}
