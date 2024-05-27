using System;

namespace Chess
{
    public static class AI
    {
        private static int currDepth;
        private const int maxDepth = 3;
        private const int winValue = 10000;
        private const int checkValue = 50;
        private const int drawValue = 0;
        private const int nonTerminalState = int.MaxValue;
        private static int recursionCalls;

        private static int CheckTerminalState(ChessboardSquare[,] squares, string turn)
        {
            if (!Chessboard.AreTherePossibleMoves(squares, turn))
            {
                if (Chessboard.IsKingInCheck(squares, turn))
                {
                    return (turn == "White") ? -winValue * maxDepth : winValue * maxDepth;
                }
                else
                {
                    return drawValue;
                }
            }

            int whiteMaterial = 0, blackMaterial = 0;
            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null)
                    {
                        if (squares[i, j].Piece is Pawn)
                        {
                            return int.MaxValue;
                        }
                        else
                        {
                            if (squares[i, j].Piece.Color == "White")
                            {
                                whiteMaterial += squares[i, j].Piece.Value;
                            }
                            else
                            {
                                blackMaterial += squares[i, j].Piece.Value;
                            }
                        }
                    }
                }
            }

            if (whiteMaterial <= 3 && blackMaterial <= 3)
            {
                return drawValue;
            }

            return nonTerminalState;
        }

        public static int[] Minimax(ChessboardSquare[,] squares, string turn)
        { 
            currDepth = 1;
            recursionCalls = 0;

            int[] optimalAction = new int[4];
            int bestValue = (turn == "Black") ? int.MaxValue : int.MinValue;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null && squares[i, j].Piece.Color == turn)
                    {
                        foreach(Tuple<int, int> move in squares[i, j].Piece.PossibleMoves(i, j, squares))
                        {
                            Piece tmpFrom, tmpTo;
                            (tmpFrom, tmpTo) = DoMove(squares, i, j, move.Item1, move.Item2);                             
                            int currValue = (turn == "Black") ? MaxPlayer(squares, bestValue) : MinPlayer(squares, bestValue);
                            if (currValue < bestValue)
                            {
                                bestValue = currValue;
                                optimalAction[0] = i;
                                optimalAction[1] = j;
                                optimalAction[2] = move.Item1; 
                                optimalAction[3] = move.Item2;
                            }
                            UndoMove(squares, i, j, move.Item1, move.Item2, tmpFrom, tmpTo);
                        }
                    }
                }
            }

            Console.WriteLine("Recursion calls: {0}", recursionCalls);

            return optimalAction;
        }

        private static int MinPlayer(ChessboardSquare[,] squares, int alpha)
        {
            recursionCalls++;

            if (CheckTerminalState(squares, "Black") != nonTerminalState)
            {
                return CheckTerminalState(squares, "Black") / currDepth;
            }
            if (currDepth == maxDepth)
            {
                return EvaluateBoard(squares);
            }

            int beta = int.MaxValue;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null && squares[i, j].Piece.Color == "Black")
                    {
                        foreach (Tuple<int, int> move in squares[i, j].Piece.PossibleMoves(i, j, squares))
                        {
                            if (beta == -winValue * maxDepth || beta < alpha)
                            {
                                return beta;
                            }

                            Piece tmpFrom, tmpTo;
                            (tmpFrom, tmpTo) = DoMove(squares, i, j, move.Item1, move.Item2);
                            currDepth++;
                            beta = Math.Min(beta, MaxPlayer(squares, beta));
                            currDepth--;
                            UndoMove(squares, i, j, move.Item1, move.Item2, tmpFrom, tmpTo);
                        }
                    }
                }
            }

            return beta;
        }

        private static int MaxPlayer(ChessboardSquare[,] squares, int alpha)
        {
            recursionCalls++;

            if (CheckTerminalState(squares, "White") != nonTerminalState)
            {
                return CheckTerminalState(squares, "White") / currDepth;
            }
            if (currDepth == maxDepth)
            {
                return EvaluateBoard(squares);
            }

            int beta = int.MinValue;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null && squares[i, j].Piece.Color == "White")
                    {
                        foreach (Tuple<int, int> move in squares[i, j].Piece.PossibleMoves(i, j, squares))
                        {
                            if (beta == winValue * maxDepth || beta > alpha)
                            {
                                return beta;
                            }

                            Piece tmpFrom, tmpTo;
                            (tmpFrom, tmpTo) = DoMove(squares, i, j, move.Item1, move.Item2);
                            currDepth++;
                            beta = Math.Max(beta, MinPlayer(squares, beta));
                            currDepth--;
                            UndoMove(squares, i, j, move.Item1, move.Item2, tmpFrom, tmpTo);
                        }
                    }
                }
            }

            return beta;
        }

        private static int EvaluateBoard(ChessboardSquare[,] squares)
        {
            int value = 0;

            if (Chessboard.IsKingInCheck(squares, "White"))
            {
                value += -checkValue;
            }
            else if (Chessboard.IsKingInCheck(squares, "Black"))
            {
                value += checkValue;
            }

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null)
                    {
                        Piece piece = squares[i, j].Piece;
                        value += ((piece.Color == "White") ? piece.Value : -piece.Value) * 100;
                        if (piece is Pawn)
                        {
                            value += (piece.Color == "White") ? Pawn.evalTable[i * 8 + j] : -Pawn.evalTable[Piece.flip[i * 8 + j]];
                        }
                        else if (piece is Bishop)
                        {
                            value += (piece.Color == "White") ? Bishop.evalTable[i * 8 + j] : -Bishop.evalTable[Piece.flip[i * 8 + j]];
                        }
                        else if (piece is Knight)
                        {
                            value += (piece.Color == "White") ? Knight.evalTable[i * 8 + j] : -Knight.evalTable[Piece.flip[i * 8 + j]];
                        }
                    }
                }
            }

            return value;
        }

        private static Tuple<Piece, Piece> DoMove(ChessboardSquare[,] squares, int fromI, int fromJ, int toI, int toJ)
        {
            Piece tmpTo = squares[toI, toJ].Piece, tmpFrom = squares[fromI, fromJ].Piece;
            ChessboardSquare sqrFrom = squares[fromI, fromJ], sqrTo = squares[toI, toJ];

            #region Pawn promotion
            if (toI == 0 && sqrFrom.Piece is Pawn && sqrFrom.Piece.Color == "White")
            {
                sqrFrom.Piece = new Queen("White");
            }
            else if (toI == 7 && sqrFrom.Piece is Pawn && sqrFrom.Piece.Color == "Black")
            {
                sqrFrom.Piece = new Queen("Black");
            }
            #endregion

            #region Castling
            if (sqrFrom.Piece is King)
            {
                if (sqrFrom.Position == "e1")
                {
                    if (sqrTo.Position == "g1")
                    {
                        squares[7, 5].Piece = squares[7, 7].Piece;
                        squares[7, 5].Piece.HasMoved = true;
                        squares[7, 7].Piece = null;
                    }
                    else if (sqrTo.Position == "c1")
                    {
                        squares[7, 3].Piece = squares[7, 0].Piece;
                        squares[7, 3].Piece.HasMoved = true;
                        squares[7, 0].Piece = null;
                    }
                }
                else if (sqrFrom.Position == "e8")
                {
                    if (sqrTo.Position == "g8")
                    {
                        squares[0, 5].Piece = squares[0, 7].Piece;
                        squares[0, 5].Piece.HasMoved = true;
                        squares[0, 7].Piece = null;
                    }
                    else if (sqrTo.Position == "c8")
                    {
                        squares[0, 3].Piece = squares[0, 0].Piece;
                        squares[0, 3].Piece.HasMoved = true;
                        squares[0, 0].Piece = null;
                    }
                }
            }
            #endregion

            sqrTo.Piece = sqrFrom.Piece;
            sqrTo.Piece.HasMoved = true;
            sqrFrom.Piece = null;

            return new Tuple<Piece, Piece>(tmpFrom, tmpTo);
        }

        private static void UndoMove(ChessboardSquare[,] squares, int fromI, int fromJ, int toI, int toJ, Piece pieceFrom, Piece pieceTo)
        {
            ChessboardSquare sqrFrom = squares[fromI, fromJ], sqrTo = squares[toI, toJ];

            #region Castling
            if (pieceFrom is King)
            {
                if (sqrFrom.Position == "e1")
                {
                    if (sqrTo.Position == "g1")
                    {
                        squares[7, 7].Piece = squares[7, 5].Piece;
                        squares[7, 7].Piece.HasMoved = false;
                        squares[7, 5].Piece = null;
                    }
                    else if (sqrTo.Position == "c1")
                    {
                        squares[7, 0].Piece = squares[7, 3].Piece;
                        squares[7, 0].Piece.HasMoved = false;
                        squares[7, 3].Piece = null;
                    }
                }
                else if (sqrFrom.Position == "e8")
                {
                    if (sqrTo.Position == "g8")
                    {
                        squares[0, 7].Piece = squares[0, 5].Piece;
                        squares[0, 7].Piece.HasMoved = false;
                        squares[0, 5].Piece = null;
                    }
                    else if (sqrTo.Position == "c8")
                    {
                        squares[0, 0].Piece = squares[0, 3].Piece;
                        squares[0, 0].Piece.HasMoved = false;
                        squares[0, 3].Piece = null;
                    }
                }
            }
            #endregion

            sqrFrom.Piece = pieceFrom;
            sqrTo.Piece = pieceTo;
            pieceFrom.HasMoved = false;
        }
    }
}
