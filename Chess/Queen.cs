using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Queen : Piece
    {
        public Queen(string color)
        {
            if (color != "White" && color != "Black")
            {
                throw new ArgumentException();
            }

            Value = 9;
            Color = color;
            Name = "queen";
            HasMoved = false;
        }

        public override Tuple<int, int>[] PossibleMoves(int i, int j, ChessboardSquare[,] squares)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

            for (int dirI = -1; dirI <= 1; dirI++)
            {
                for (int dirJ = -1; dirJ <= 1; dirJ++)
                {
                    if (dirI == 0 && dirJ == 0) continue;

                    int mult = 1;
                    int di = dirI, dj = dirJ;

                    while ((0 <= i + di && i + di <= 7) && (0 <= j + dj && j + dj <= 7))
                    {
                        #region Check if king will be under a check
                        Piece tmpPiece = squares[i + di, j + dj].Piece;
                        squares[i + di, j + dj].Piece = this;
                        squares[i, j].Piece = null;

                        if (Chessboard.IsKingInCheck(squares, Color))
                        {
                            squares[i + di, j + dj].Piece = tmpPiece;
                            squares[i, j].Piece = this;
                            if (squares[i + di, j + dj].Piece == null)
                            {
                                mult++;
                                di = dirI * mult;
                                dj = dirJ * mult;
                                continue;
                            }
                            else
                            {
                                break;
                            }
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
                            break;
                        }

                        moves.Add(new Tuple<int, int>(i + di, j + dj));
                        mult++;
                        di = dirI * mult;
                        dj = dirJ * mult;
                    }
                }
            }

            return moves.ToArray();
        }

        public override void MarkSeen(int i, int j, ChessboardSquare[,] squares)
        {
            for (int dirI = -1; dirI <= 1; dirI++)
            {
                for (int dirJ = -1; dirJ <= 1; dirJ++)
                {
                    if (dirI == 0 && dirJ == 0) continue;

                    int mult = 1;
                    int di = dirI, dj = dirJ;

                    while ((0 <= i + di && i + di <= 7) && (0 <= j + dj && j + dj <= 7))
                    {
                        if (Color == "White")
                        {
                            squares[i + di, j + dj].IncreaseWhiteSeenCount();
                        }
                        else
                        {
                            squares[i + di, j + dj].IncreaseBlackSeenCount();
                        }

                        if (squares[i + di, j + dj].Piece != null)
                        {
                            break;
                        }

                        mult++;
                        di = dirI * mult;
                        dj = dirJ * mult;
                    }
                }
            }
        }
    }
}
