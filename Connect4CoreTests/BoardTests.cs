﻿using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace connect_4_core.Tests
{
    [TestClass()]
    public class BoardTests
    {
        Board b = new Board();
        Location loc00 = new Location(0, 0);
        Location loc15 = new Location(1, 5);
        Location loc25 = new Location(2, 5);
        Location loc02 = new Location(0, 2);
        Location loc60 = new Location(6, 0);
        Location loc35 = new Location(3, 5);
        Location loc44 = new Location(4, 4);
        Location loc05 = new Location(0, 5);
        Location loc32 = new Location(3, 2);




        [TestInitialize()]
        public void TestInitialize()
        {
            b = new Board();
        }

        [TestMethod()]
        public void DropPieceSuccess_Test()
        {
            Assert.AreEqual(PlayerID.None, b.GetPlayer(loc25));
            b.DropPiece(2, PlayerID.Two);
            Assert.AreEqual(PlayerID.Two, b.GetPlayer(loc25));
        }

        [TestMethod()]
        public void DropPieceInFullCol_Test()
        {
            Assert.AreEqual(PlayerID.None, b.GetPlayer(loc25));
            // Drop 6 pieces in column 2
            for (uint i = 0; i < 6; i++)
            {
                b.DropPiece(2, PlayerID.Two);
            }
            Assert.IsTrue(b.IsColFull(2));

            // Drop another price, expect an Exception
            try
            {
                b.DropPiece(2, PlayerID.Two);
                // Should never get here
                Assert.Fail();
            } catch (Exception)
            {                                
            }
        }

        [TestMethod()]
        public void CheckWinTopLeftBottomRight_Test()
        {
            var win = b.CheckWin(0, PlayerID.One);
            Assert.IsFalse(win);
            // 

            List<int[]> p1List = new List<int[]>();
            p1List.Add(new int[] { 0, 0 });
            p1List.Add(new int[] { 1, 1 });
            p1List.Add(new int[] { 2, 2 });
            p1List.Add(new int[] { 3, 3 });

            List<int[]> p2List = new List<int[]>();
            b.Populate(p1List, p2List);

            win = b.CheckWin(0, PlayerID.One);
            Assert.IsTrue(win);

            p2List.Add(new int[] { 2, 2 });
            b.Populate(p1List, p2List);
            win = b.CheckWin(loc00.Col, PlayerID.One);
            Assert.IsFalse(win);
        }

        [TestMethod()]
        public void CheckWinBottomLeftTopRight_Test()
        {
            var win = b.CheckWin(loc15.Col, PlayerID.One);
            Assert.IsFalse(win);
            // 

            List<int[]> p1List = new List<int[]>();
            p1List.Add(new int[] { 0, 5 });
            p1List.Add(new int[] { 1, 4 });
            p1List.Add(new int[] { 2, 3 });
            p1List.Add(new int[] { 3, 2 });

            List<int[]> p2List = new List<int[]>();
            b.Populate(p1List, p2List);

            win = b.CheckWin(1, PlayerID.One);
            Assert.IsTrue(win);

            p2List.Add(new int[] { 4, 2 });
            b.Populate(p1List, p2List);
            Assert.IsTrue(win);

            p1List = new List<int[]>();
            p1List.Add(new int[] { 0, 5 });
            p1List.Add(new int[] { 1, 4 });
            p1List.Add(new int[] { 2, 3 });
            p1List.Add(new int[] { 3, 2 });

            b.Populate(p1List, p2List);
            win = b.CheckWin(loc05.Col, PlayerID.One);
            Assert.IsTrue(win);
        }
    }
}