﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace connect_4_core
{
    public static class VictoryChecker
    {
        private static uint getRow(Board board, uint col)
        {
            var row = board.FindTopRow(col) + 1;
            return row > 6 ? 0 : row;
        }

        private static bool CheckVerticalWin(Board board, uint col, PlayerID player)
        {
            var row = board.FindTopRow(col) + 1;
            if (row == Board.INVALID_ROW + 1)
            {
                row = 0;
            }
            if (row > 2)
            {
                return false;
            }

            for (uint i = row; i < row + 4; i++)
            {
                var p = board.Get(col, i);
                if (p != player) {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckHorizontalWin(Board board, uint col, PlayerID player)
        {
            uint counter = 1;
            var row = getRow(board, col);
            if (row > 5) {
                return false;
            }

            // Left check
            for (uint i = 1; i < 4; i++)
            {
                if (col < i)
                {
                    break;
                }
                if (board.GetPlayer(new Location(col - i, row)) != player)
                {
                    break;
                }

                counter++;
            }

            if (counter >= 4) {
                return true;
            }

            // Right check
            for (uint i = 1; i < 4; i++)
            {
                if (col + i > 6)
                {
                    break;
                }
                if (board.GetPlayer(new Location(col + i, row)) != player)
                {
                    break;
                }

                counter++;
            }

            return counter >= 4;
        }

        private static bool CheckTopLeftBotRightWin(Board board, uint col, PlayerID player)
        {
            uint counter = 1;
            var row = getRow(board, col);
            // Top left check
            for (uint i = 1; i < 4; i++)
            {
                if (col < i || row < i)
                {
                    break;
                }
                if (board.GetPlayer(new Location(col - i, row - i)) != player)
                {
                    break;
                }

                counter++;
            }

            if (counter >= 4) {
                return true;
            }

            // Bottom right check
            for (uint i = 1; i < 4; i++)
            {
                if (col + i > 6 || row + i > 5)
                {
                    break;
                }
                if (board.GetPlayer(new Location(col + i, row + i)) != player)
                {
                    break;
                }

                counter++;
            }

            return counter >= 4;
        }

        private static bool CheckBotLeftTopRightWin(Board board, uint col, PlayerID player)
        {
            uint counter = 1;
            var row = getRow(board, col);

            // Top right check
            for (uint i = 1; i < 4; i++)
            {
                if (col + i > 6 || row < i)
                {
                    break;
                }
                if (board.GetPlayer(new Location(col + i, row - i)) != player)
                {
                    break;
                }

                counter++;
            }

            if (counter >= 4)
            {
                return true;
            }

            // Bottom left check
            for (uint i = 1; i < 4; i++)
            {
                if (col < i || row + i > 5)
                {
                    break;
                }
                if (board.GetPlayer(new Location(col - i, row + i)) != player)
                {
                    break;
                }

                counter++;
            }

            return counter >= 4;
        }

        public static bool CheckVictory(Board board, uint col, PlayerID player)
        {
            if (CheckVerticalWin(board, col, player)) 
            {
                return true;
            } 
            if (CheckHorizontalWin(board, col, player))
            {
                return true;
            }
            if (CheckTopLeftBotRightWin(board, col, player))
            {
                return true;
            }
            if (CheckBotLeftTopRightWin(board, col, player))
            {
                return true;
            }

            return false;
        }
    }
}
