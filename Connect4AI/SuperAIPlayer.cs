﻿using connect_4_core;
using Connect4AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Connect4AI
{
    enum BoardScore : uint
    {
        SureVictory, // 2 places to win
        PossibleVictory, // 1 place to win
        Unknown // Nothing
    }

    public class SuperAIPlayer : IPlayer
    {
        PlayerID aiPlayer;
        PlayerID human;
        uint lookAhead;
        public SuperAIPlayer(PlayerID player, uint lookAhead)
        {
            aiPlayer = player;
            human = aiPlayer.Other();
            this.lookAhead = lookAhead;
        }

        BoardGenerator boardGenerator = new BoardGenerator();

        Random rnd = new Random();
        public uint Play(Board board)
        {
            Thread.Sleep(1000);

            // Check if AI wins
            var cols = FindWinningCols(board, aiPlayer);
            if (cols.Count > 0) {
                return cols.First();
            }

            // Check if human wins
            cols = FindWinningCols(board, human);
            if (cols.Count > 0)
            {
                return cols.First();
            }

            var pCols = FindPotentialCols(board);

            uint index = (uint)rnd.Next(pCols.Length);
            return pCols[index];
        }


        public uint[] FindPotentialCols(Board b)
        {
            var cols = new HashSet<uint>();
            // Generate all boards
            var boards = boardGenerator.GenerateAllBoards(b, new PlaySequenceSet(), lookAhead, aiPlayer);
            // Evaluate each board
            var allScores = new Dictionary<SuperBoard, BoardScore>();
            foreach (var e in boards)
            {
                var bo = e.Value.Board;
                var bs = EvaluateBoard(bo, aiPlayer);
                Debug.WriteLine(bo.ToString(), bs);
                Debug.WriteLine("-------");
                allScores.Add(e.Value, bs);
            }

            // Drop boards with unknown board score
            var scores = allScores.Where(pair => pair.Value != BoardScore.Unknown)
                                 .ToDictionary(pair => pair.Key,
                                               pair => pair.Value);
            // Find boards with sure or potential victory
            var potentialVictoryScore = new HashSet<BoardScore>{ BoardScore.SureVictory, BoardScore.PossibleVictory };
            var victory = scores.Where(pair => potentialVictoryScore.Contains(pair.Value))
                                     .Select(pair => pair.Key).ToList();

            // Return first col from each play sequence
            foreach (var board in victory)
            {
                foreach (var sequence in board.PlaySequenceSet)
                {
                    cols.Add(sequence.First);
                }
            }

            return cols.ToArray();
        }

        // Finds columns that lead to a sure victory for AI player or human player on their next turn
        private Dictionary<uint, Board> CheckBoards(HashSet<Board> boards, PlayerID player)
        {
            var result = new Dictionary<uint, Board>();
            foreach(Board b in boards)
            {
                var cols = FindWinningCols(b, player);
            }

            return result;
        }

        private HashSet<uint> FindWinningCols(Board board, PlayerID player)
        {
            var cols = new HashSet<uint>();
            for (uint i = 0; i < 7; i++)
            {
                //check if column is full
                if (board.IsColFull(i)) {
                    continue;
                }

                //clone board and drop piece in current col
                var b = new Board(board);
                b.DropPiece(i, player);

                //if top row is replaced with ai piece checks if the ai will win
                if (VictoryChecker.CheckVictory(b, i, player))
                {
                    cols.Add(i);
                }
            }
            return cols;
        }

        private BoardScore EvaluateBoard(Board b, PlayerID p)
        {
            var sb = new SuperBoard(b, new PlaySequenceSet());
            var bg = new BoardGenerator();
            var boards = bg.GenerateBoards(sb, p);
            uint counter = 0;

            foreach (var e in boards) {
                for (uint i = 0; i < 7; i++)
                {
                    if (VictoryChecker.CheckVictory(e.Value.Board, i, p.Other()))
                    {
                        counter++;
                        break;
                    }

                    // Check if we can win for sure on our next turn
                    var bb = new Board(e.Value.Board);
                    if (bb.IsColFull(i))
                    {
                        continue; 
                    }
                    bb.DropPiece(i, p);

                    for (uint j = 0; j < 7; j++)
                    {
                        var result = VictoryChecker.CheckVictory(bb, j, p);
                        if (result)
                        {
                            // If other player blocks us on column j, check if we can win on the row above

                            if (bb.IsColFull(j))
                            {
                                continue;
                            }
                            bb.DropPiece(j, p.Other());

                            if (bb.IsColFull(j))
                            {
                                continue;
                            }
                            bb.DropPiece(j, p);

                            if (VictoryChecker.CheckVictory(bb, j, p))
                            {
                                counter++;
                                break;
                            }
                        }
                    }
                }
            }

            if (counter == boards.Count)
            {
                return BoardScore.SureVictory;
            }

            return BoardScore.Unknown;
        }
    }
}