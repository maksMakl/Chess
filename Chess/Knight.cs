using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Knight : Piece
    {
        public static int[] evalTable =
        {
            -10, -10, -10, -10, -10, -10, -10, -10,
            -10,   0,   0,   0,   0,   0,   0, -10,
            -10,   0,   5,   5,   5,   5,   0, -10,
            -10,   0,   5,  10,  10,   5,   0, -10,
            -10,   0,   5,  10,  10,   5,   0, -10,
            -10,   0,   5,   5,   5,   5,   0, -10,
            -10,   0,   0,   0,   0,   0,   0, -10,
            -10, -30, -10, -10, -10, -10, -30, -10
        };


        public Knight(string color)
        {
            if (color != "White" && color != "Black")
            {
                throw new ArgumentException();
            }

            Value = 3;
            Color = color;
            Name = "knight";
            HasMoved = false;
        }

        public override Tuple<int, int>[] PossibleMoves(int i, int j, ChessboardSquare[,] squares)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
            int[] directions = new int[4] { -2, -1, 1, 2 };

            foreach (int di in directions)
            {
                foreach (int dj in directions)
                {
                    if (Math.Abs(di) == Math.Abs(dj)) continue;

                    if ((0 <= i + di && i + di <= 7) && (0 <= j + dj && j + dj <= 7))
                    {
                        #region Check if king will be under a check
                        Piece tmpPiece = squares[i + di, j + dj].Piece;
                        squares[i + di, j + dj].Piece = this;
                        squares[i, j].Piece = null;

                        if (Chessboard.IsKingInCheck(squares, Color))
                        {
                            squares[i + di, j + dj].Piece = tmpPiece;
                            squares[i, j].Piece = this;
                            continue;
                        }
                        squares[i + di, j + dj].Piece = tmpPiece;
                        squares[i, j].Piece = this;
                        #endregion

                        if (squares[i + di, j + dj].Piece != null)
                        {
                            if (squares[i + di, j + dj].Piece.Color != Color)
                            {
                                moves.Add(new Tuple<int, int>(i + di, j + dj));
                            }
                        }
                        else
                        {
                            moves.Add(new Tuple<int, int>(i + di, j + dj));
                        }
                    }
                }
            }

            return moves.ToArray();
        }

        public override void MarkSeen(int i, int j, ChessboardSquare[,] squares)
        {
            int[] directions = new int[4] { -2, -1, 1, 2 };

            foreach (int di in directions)
            {
                foreach (int dj in directions)
                {
                    if (Math.Abs(di) == Math.Abs(dj)) continue;

                    if ((0 <= i + di && i + di <= 7) && (0 <= j + dj && j + dj <= 7))
                    {
                        if (Color == "White")
                        {
                            squares[i + di, j + dj].IncreaseWhiteSeenCount();
                        }
                        else
                        {
                            squares[i + di, j + dj].IncreaseBlackSeenCount();
                        }
                    }
                }
            }
        }
    }
}
