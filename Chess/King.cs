using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chess
{
    public class King : Piece
    {
        public King(string color)
        {
            if (color != "White" && color != "Black")
            {
                throw new ArgumentException();
            }

            Value = 0;
            Color = color;
            Name = "king";
            HasMoved = false;
        }

        public override Tuple<int, int>[] PossibleMoves(int i, int j, ChessboardSquare[,] squares)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

            for (int di = -1; di <= 1; di++)
            {
                for (int dj = -1; dj <= 1; dj++)
                {
                    if (di == 0 && dj == 0) continue;

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

            #region Castling
            if (HasMoved || Chessboard.IsKingInCheck(squares, Color)) return moves.ToArray();

            if (i == 7 && j == 4 && Color == "White")
            {
                if (squares[7, 5].Piece == null && squares[7, 6].Piece == null && squares[7, 7].Piece is Rook && !squares[7, 7].Piece.HasMoved && !squares[7, 5].SeenByBlack && !squares[7, 6].SeenByBlack)
                {
                    moves.Add(new Tuple<int, int>(7, 6));
                }

                if (squares[7, 3].Piece == null && squares[7, 2].Piece == null && squares[7, 1].Piece == null && squares[7, 0].Piece is Rook && !squares[7, 0].Piece.HasMoved && !squares[7, 3].SeenByBlack && !squares[7, 2].SeenByBlack)
                {
                    moves.Add(new Tuple<int, int>(7, 2));
                }
            }
            else if (i == 0 && j == 4 && Color == "Black")
            {
                if (squares[0, 5].Piece == null && squares[0, 6].Piece == null && squares[0, 7].Piece is Rook && !squares[0, 7].Piece.HasMoved && !squares[0, 5].SeenByWhite && !squares[0, 6].SeenByWhite)
                {
                    moves.Add(new Tuple<int, int>(0, 6));
                }

                if (squares[0, 3].Piece == null && squares[0, 2].Piece == null && squares[0, 1].Piece == null && squares[0, 0].Piece is Rook && !squares[0, 0].Piece.HasMoved && !squares[0, 3].SeenByWhite && !squares[0, 2].SeenByWhite)
                {
                    moves.Add(new Tuple<int, int>(0, 2));
                }
            }
            #endregion

            return moves.ToArray();
        }

        public override void MarkSeen(int i, int j, ChessboardSquare[,] squares)
        {
            for (int di = -1; di <= 1; di++)
            {
                for (int dj = -1; dj <= 1; dj++)
                {
                    if (di == 0 && dj == 0) continue;

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

