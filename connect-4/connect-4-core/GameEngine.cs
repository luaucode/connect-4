using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace connect_4_core
{
    public class GameEngine : IGameEngine
    {
        IBoard board;

        Random rnd;
        int activePlayer;

        int winner = -1;

        public GameEngine()
        {
            board = new Board();
            rnd = new Random();
            activePlayer = rnd.Next(2);
        }
        public int GetActivePlayer()
        {
            return activePlayer;
        }

        public int GetWinner()
        {
            return winner;
        }

        public IBoard GetBoard()
        {
            return board;
        }


        public void Run(IPlayer p1, IPlayer p2, IGameEngineEvents sink)
        {
            IPlayer[] players = {p1, p2};
            
            int col = 0;
            int row = 0;
            

            while (!IsGameOver(col, row)) {
                activePlayer = activePlayer == 0 ? 1 : 0;
                col = players[activePlayer].Play(board);
                row = board.DropPiece(col, activePlayer);
                sink.OnDropPiece(row, col);
            }
            sink.OnGameOver(GetWinner());
        }

        private bool IsGameOver(int col, int row)
        {
            int counter = 0;
            for (int i = 0; i < 7; i++)
            {
                if (board.IsColFull(i))
                {
                    counter++;
                }
            }

            if (board.CheckRowWin(row, activePlayer))
            {
                winner = activePlayer;
                return true;
            } else if (board.CheckColWin(col, activePlayer))
            {
                winner = activePlayer;
                return true;
            }
                
            return counter == 7;
        }
    }
}
