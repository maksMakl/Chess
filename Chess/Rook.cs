using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class Rook : Piece
    {
        public Rook(string color) 
        {
            if (color != "White" && color != "Black")
            {
                throw new ArgumentException();
            }

            Value = 5;
            Color = color;
            Name = "rook";
            HasMoved = false;
        }

        public override Tuple<int, int>[] PossibleMoves(int i, int j, ChessboardSquare[,] squares)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

            for (int up = i - 1; up >= 0; up--)
            {
                ChessboardSquare square = squares[up, j];

                #region Check if king will be under a check
                Piece tmpPiece = square.Piece;
                square.Piece = this;
                squares[i, j].Piece = null;

                if (Chessboard.IsKingInCheck(squares, Color))
                {
                    square.Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    if (square.Piece == null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                square.Piece = tmpPiece;
                squares[i, j].Piece = this;
                #endregion

                if (square.Piece == null)
                {
                    moves.Add(new Tuple<int, int>(up, j));
                }
                else
                {
                    if (Color != square.Piece.Color)
                    {
                        moves.Add(new Tuple<int, int>(up, j));
                    }
                    break;
                }
            }

            for (int down = i + 1; down < 8; down++)
            {
                ChessboardSquare square = squares[down, j];

                #region Check if king will be under a check
                Piece tmpPiece = square.Piece;
                square.Piece = this;
                squares[i, j].Piece = null;

                if (Chessboard.IsKingInCheck(squares, Color))
                {
                    square.Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    if (square.Piece == null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                square.Piece = tmpPiece;
                squares[i, j].Piece = this;
                #endregion

                if (square.Piece == null)
                {
                    moves.Add(new Tuple<int, int>(down, j));
                }
                else
                {
                    if (Color != square.Piece.Color)
                    {
                        moves.Add(new Tuple<int, int>(down, j));
                    }
                    break;
                }
            }

            for (int left = j - 1; left >= 0; left--)
            {
                ChessboardSquare square = squares[i, left];

                #region Check if king will be under a check
                Piece tmpPiece = square.Piece;
                square.Piece = this;
                squares[i, j].Piece = null;

                if (Chessboard.IsKingInCheck(squares, Color))
                {
                    square.Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    if (square.Piece == null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                square.Piece = tmpPiece;
                squares[i, j].Piece = this;
                #endregion

                if (square.Piece == null)
                {
                    moves.Add(new Tuple<int, int>(i, left));
                }
                else
                {
                    if (Color != square.Piece.Color)
                    {
                        moves.Add(new Tuple<int, int>(i, left));
                    }
                    break;
                }
            }

            for (int right = j + 1; right < 8; right++)
            {
                ChessboardSquare square = squares[i, right];

                #region Check if king will be under a check
                Piece tmpPiece = square.Piece;
                square.Piece = this;
                squares[i, j].Piece = null;

                if (Chessboard.IsKingInCheck(squares, Color))
                {
                    square.Piece = tmpPiece;
                    squares[i, j].Piece = this;
                    if (square.Piece == null)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                square.Piece = tmpPiece;
                squares[i, j].Piece = this;
                #endregion

                if (square.Piece == null)
                {
                    moves.Add(new Tuple<int, int>(i, right));
                }
                else
                {
                    if (Color != square.Piece.Color)
                    {
                        moves.Add(new Tuple<int, int>(i, right));
                    }
                    break;
                }
            }

            return moves.ToArray();
        }

        public override void MarkSeen(int i, int j, ChessboardSquare[,] squares)
        {
            for (int up = i - 1; up >= 0; up--)
            {
                ChessboardSquare square = squares[up, j];

                if (Color == "White")
                {
                    square.IncreaseWhiteSeenCount();
                }
                else
                {
                    square.IncreaseBlackSeenCount();
                }

                if (square.Piece != null)
                {
                    break;
                }
            }

            for (int down = i + 1; down < 8; down++)
            {
                ChessboardSquare square = squares[down, j];

                if (Color == "White")
                {
                    square.IncreaseWhiteSeenCount();
                }
                else
                {
                    square.IncreaseBlackSeenCount();
                }

                if (square.Piece != null)
                {
                    break;
                }
            }

            for (int left = j - 1; left >= 0; left--)
            {
                ChessboardSquare square = squares[i, left];

                if (Color == "White")
                {
                    square.IncreaseWhiteSeenCount();
                }
                else
                {
                    square.IncreaseBlackSeenCount();
                }

                if (square.Piece != null)
                {
                    break;
                }
            }

            for (int right = j + 1; right < 8; right++)
            {
                ChessboardSquare square = squares[i, right];

                if (Color == "White")
                {
                    square.IncreaseWhiteSeenCount();
                }
                else
                {
                    square.IncreaseBlackSeenCount();
                }

                if (square.Piece != null)
                {
                    break;
                }
            }
        }
    }
}
