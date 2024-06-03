using System;
using System.Collections.Generic;

namespace Chess
{
    public class Pawn : Piece
    {
        public static int[] evalTable = 
        {
            0,   0,   0,   0,   0,   0,   0,   0,
            5,  10,  15,  20,  20,  15,  10,   5,
            4,   8,  12,  16,  16,  12,   8,   4,
            3,   6,   9,  12,  12,   9,   6,   3,
            2,   4,   6,   8,   8,   6,   4,   2,
            1,   2,   3, -10, -10,   3,   2,   1,
            0,   0,   0, -40, -40,   0,   0,   0,
            0,   0,   0,   0,   0,   0,   0,   0
        };


        public Pawn(string color)
        {
            if (color != "White" && color != "Black")
            {
                throw new ArgumentException();
            }

            Value = 1;
            Color = color;
            Name = "pawn";
            HasMoved = false;
        }

        public override Tuple<int, int>[] PossibleMoves(int i, int j, ChessboardSquare[,] squares)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
            int dir = (Color == "White") ? -1 : 1;

            if (0 <= i + dir && i + dir <= 7)
            {
                if (squares[i + dir, j].Piece == null)
                {
                    #region Check if king will be under a check
                    Piece tmpPiece = squares[i + dir, j].Piece;
                    squares[i + dir, j].Piece = this;
                    squares[i, j].Piece = null;

                    if (!Chessboard.IsKingInCheck(squares, Color))
                    {
                        moves.Add(new Tuple<int, int>(i + dir, j));
                    }
                    squares[i + dir, j].Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    #endregion
                }

                if (0 <= j + 1 && j + 1 <= 7 && squares[i + dir, j + 1].Piece != null && squares[i + dir, j + 1].Piece.Color != Color)
                {
                    #region Check if king will be under a check
                    Piece tmpPiece = squares[i + dir, j + 1].Piece;
                    squares[i + dir, j + 1].Piece = this;
                    squares[i, j].Piece = null;

                    if (!Chessboard.IsKingInCheck(squares, Color))
                    {
                        moves.Add(new Tuple<int, int>(i + dir, j + 1));
                    }
                    squares[i + dir, j + 1].Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    #endregion
                }
                if (0 <= j - 1 && j - 1 <= 7 && squares[i + dir, j - 1].Piece != null && squares[i + dir, j - 1].Piece.Color != Color)
                {
                    #region Check if king will be under a check
                    Piece tmpPiece = squares[i + dir, j - 1].Piece;
                    squares[i + dir, j - 1].Piece = this;
                    squares[i, j].Piece = null;

                    if (!Chessboard.IsKingInCheck(squares, Color))
                    {
                        moves.Add(new Tuple<int, int>(i + dir, j - 1));
                    }
                    squares[i + dir, j - 1].Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    #endregion
                }
            }
            if (i == (dir == -1 ? 6 : 1) && squares[i + dir, j].Piece == null && squares[i + dir * 2, j].Piece == null)
            {
                #region Check if king will be under a check
                Piece tmpPiece = squares[i + 2 * dir, j].Piece;
                squares[i + 2 * dir, j].Piece = this;
                squares[i, j].Piece = null;

                if (!Chessboard.IsKingInCheck(squares, Color))
                {
                    moves.Add(new Tuple<int, int>(i + 2 * dir, j));
                }
                squares[i + 2 * dir, j].Piece = tmpPiece;
                squares[i, j].Piece = this;
                #endregion
            }

            return moves.ToArray();
        }

        public override void MarkSeen(int i, int j, ChessboardSquare[,] squares)
        {
            int dir = (Color == "White") ? -1 : 1;

            if (0 <= i + dir && i + dir <= 7)
            {

                if (0 <= j + 1 && j + 1 <= 7)
                {
                    if (Color == "White")
                    {
                        squares[i + dir, j + 1].IncreaseWhiteSeenCount();
                    }
                    else
                    {
                        squares[i + dir, j + 1].IncreaseBlackSeenCount();
                    }
                }
                if (0 <= j - 1 && j - 1 <= 7)
                {
                    if (Color == "White")
                    {
                        squares[i + dir, j - 1].IncreaseWhiteSeenCount();
                    }
                    else
                    {
                        squares[i + dir, j - 1].IncreaseBlackSeenCount();
                    }
                }
            }
        }
    }
}
