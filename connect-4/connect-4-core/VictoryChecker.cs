﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace connect_4_core
{
    public class VictoryChecker
    {
        public bool CheckVerticalWin(IBoard board, uint col, uint player)
        {
            uint topRow = board.FindTopRow(col);

            if (topRow > 2)
            {
                return false;
            }

            for (uint i = topRow + 1; i < 4 + topRow; i++)
            {
                if (board.GetPlayer(new Location(col, i)) != player) {
                    return false;
                }
            }
            return true;
        }

        public bool CheckHorizontalWin(IBoard board, uint col, uint player)
        {
            uint counter = 1;
            uint row = board.FindTopRow(col) + 1;

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

            if (counter == 4) {
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

            return counter == 4;
        }

        public bool CheckTopLeftBotRightWin(IBoard board, uint col, uint player)
        {
            uint counter = 1;
            var row = getTopRow(board, col);

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

            if (counter == 4) {
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

            return counter == 4;
        }

        private uint getTopRow(IBoard board, uint col)
        {
            uint row = board.FindTopRow(col) + 1;
            return row > 5 ? 0 : row;
        }

        public bool CheckBotLeftTopRightWin(IBoard board, uint col, uint player)
        {
            uint counter = 1;
            var row = getTopRow(board, col);

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

            if (counter == 4)
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

            return counter == 4;
        }
    }
}
