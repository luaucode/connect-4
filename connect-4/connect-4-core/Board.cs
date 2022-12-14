using System.Numerics;

namespace connect_4_core
{
    public class Board : IBoard
    {
        int?[,] board = new int?[7, 6];

        public int DropPiece(int col, int player)
        {
            var topRow = FindTopRow(col);
            if (topRow == -1)
            {
                throw new Exception("Col full");
            }
            board[col, topRow] = player;

            return topRow;
        }

        public bool IsColFull(int col)
        {
            return FindTopRow(col) == -1;
        }
        
        public int? GetPlayer(int col, int row)
        {
            return board[col, row];
        }

        //public bool CheckVictory(int player, int lastPieceRow, int lastPieceCol)
        //{

        //}

        // Finds next available row to place piece

        public int FindTopRow(int col)
        {
            for (int row = 5; row >= 0; row--)
            {
                if (board[col, row] == null)
                {
                    return row;
                }
            }
            return -1;
        }

        public bool CheckRowWin(int row, int player)
        {
            var count = 0;
            for (int col = 0; col < 7; col++)
            {
                if (board[col, row] == player)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        public bool CheckColWin(int col, int player)
        {
            var count = 0;
            for (int row = 0; row < 6; row++)
            {
                if (board[col, row] == player)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        public bool CheckTopLeftBotRightWin(int initCol, int initRow, int player)
        {
            var count = 0;
            var steps = initRow == 0 ? 6 - initCol : 6 - initRow;
            for (int i = 0; i < steps; i++)
            {
                if (board[initCol + i, initRow + i] == player)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        public bool CheckTopRightBotLeftWin(int initCol, int initRow, int player)
        {
            var count = 0;
            var steps = initCol == 0 ? initRow + 1 : Math.Min
                (initCol + 1, 6);
            var dx = initCol == 0 ? 1 : -1;
            var dy = initRow == 0 ? 1 : -1;

            if (initRow * initCol != 0) {
                throw new Exception("One of initRow and initCol must be zero");
            }

            if (initRow + initCol < 3) {
                return false;
            }

            for (int i = 0; i < steps; i++)
            {
                var row = initRow + i * dy;
                var col = initCol + i * dx;
                if (board[col, row] == player)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Populate the board with some pieces for testing
        /// </summary>
        public void Populate(List<int[]> player1Pieces, List<int[]> player2Pieces)
        {
            player1Pieces.ForEach(item =>
            {
                var row = item[1];
                var col = item[0];
                board[col, row] = 0;
            });

            player2Pieces.ForEach(item =>
            {
                var row = item[1];
                var col = item[0];
                board[col, row] = 1;
            });

        }

        public int?[,] GetBoard()
        {
            return board;
        }

        //static bool hasPlayerRow(int step, int player)
        //{

        //}

        //private bool GenericCheck(Predicate<KeyValuePair <int, int>> p, int player, int steps)
        //{
        //    var count = 0;
        //    for (int i = 0; i < steps; i++)
        //    {
        //        var pair = new KeyValuePair<int, int>(i, player);
        //        if (p(pair))
        //        {
        //            count++;
        //            if (count == 4)
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            count = 0;
        //        }
        //    }
        //    return false;
        //}
    }
}