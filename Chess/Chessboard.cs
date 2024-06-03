using System;
using System.Windows.Forms;

namespace Chess
{
    public class Chessboard
    {
        public ChessboardSquare[,] squares;
        private Tuple<int, int> activeSquareKey;

        public int Dx { get; set; }
        public int Dy { get; set; }
        public int SquareSideLen { get; set; }
        public string Turn { get; set; }

        public Chessboard()
        {
            squares = new ChessboardSquare[8, 8];
            activeSquareKey = new Tuple<int, int>(-1, -1);
            Turn = "White";
        }

        public void OnClick(int i, int j)
        {
            ChessboardSquare chosenSquare = squares[i, j];
            ChessboardSquare activeSquare = (activeSquareKey.Item1 == -1) ? null : squares[activeSquareKey.Item1, activeSquareKey.Item2];

            if (activeSquare == null)
            {
                if (chosenSquare.Piece != null && chosenSquare.Piece.Color == Turn)
                {
                    chosenSquare.Piece.MarkSquares(i, j, squares);
                    activeSquareKey = new Tuple<int, int>(i, j);
                }
            }
            else
            {
                if (chosenSquare.Marked)
                {
                    activeSquare.Piece.UnmarkSquares(activeSquareKey.Item1, activeSquareKey.Item2, squares);
                    activeSquareKey = new Tuple<int, int>(-1, -1);
                    MovePiece(activeSquare, chosenSquare);
                    CheckGameEnding();
                }
                else
                {   
                    activeSquare.Piece.UnmarkSquares(activeSquareKey.Item1, activeSquareKey.Item2, squares);
                    if (chosenSquare.Piece == null || chosenSquare.Piece.Color != Turn)
                    {
                        activeSquareKey = new Tuple<int, int>(-1, -1);
                    }
                    else
                    {
                        chosenSquare.Piece.MarkSquares(i, j, squares);
                        activeSquareKey = new Tuple<int, int>(i, j);
                    }
                }
            }
        }

        private void MovePiece(ChessboardSquare sqrFrom, ChessboardSquare sqrTo)
        {
            if (sqrFrom.Piece is King)
            {
                if (sqrFrom.Position == "e1")
                {
                    if (sqrTo.Position == "g1")
                    {
                        squares[7, 5].SetPiece(squares[7, 7].Piece);
                        squares[7, 5].Piece.HasMoved = true;
                        squares[7, 7].SetDefault();
                    }
                    else if (sqrTo.Position == "c1")
                    {
                        squares[7, 3].SetPiece(squares[7, 0].Piece);
                        squares[7, 3].Piece.HasMoved = true;
                        squares[7, 0].SetDefault();
                    }
                }
                else if (sqrFrom.Position == "e8")
                {
                    if (sqrTo.Position == "g8")
                    {
                        squares[0, 5].SetPiece(squares[0, 7].Piece);
                        squares[0, 5].Piece.HasMoved = true;
                        squares[0, 7].SetDefault();
                    }
                    else if (sqrTo.Position == "c8")
                    {
                        squares[0, 3].SetPiece(squares[0, 0].Piece);
                        squares[0, 3].Piece.HasMoved = true;
                        squares[0, 0].SetDefault();
                    }
                }
            }

            sqrTo.SetPiece(sqrFrom.Piece);
            sqrTo.Piece.HasMoved = true;
            sqrFrom.SetDefault();
            Turn = (Turn == "White") ? "Black" : "White";
        }

        public void CheckGameEnding()
        {
            if (!AreTherePossibleMoves(squares, Turn))
            {
                if (IsKingInCheck(squares, Turn))
                {
                    MessageBox.Show(String.Format("{0} won!", (Turn == "White") ? "Black" : "White"), "Checkmate!");
                }
                else
                {
                    MessageBox.Show("Tie!", "Stalemate!");
                }
                Application.Restart();
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
                            return;
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
                MessageBox.Show("Tie!", "Insufficient meterials.");
                Application.Restart();
            }
        }

        static public bool IsKingInCheck(ChessboardSquare[,] squares, string color)
        {
            ChessboardSquare kingSquare = null;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    squares[i, j].ResetSeenCount();

                    if (squares[i, j].Piece != null && squares[i, j].Piece is King && squares[i, j].Piece.Color == color)
                    {
                        kingSquare = squares[i, j];
                    }
                }
            }
            if (kingSquare == null)
            {
                return false;
            }

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null)
                    {
                        squares[i, j].Piece.MarkSeen(i, j, squares);
                    }
                }
            }


            return (color == "White") ? kingSquare.SeenByBlack : kingSquare.SeenByWhite;
        }

        public static bool AreTherePossibleMoves(ChessboardSquare[,] squares, string color)
        {
            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null && squares[i, j].Piece.Color == color && squares[i, j].Piece.PossibleMoves(i, j, squares).Length != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsValid()
        {
            int whiteKingCount = 0, blackKingCount = 0;
            int whiteRookCount = 0, blackRookCount = 0;
            int whiteBishopCount = 0, blackBishopCount = 0;
            int whiteKnightCount = 0, blackKnightCount = 0;
            int whiteQueenCount = 0, blackQueenCount = 0;
            int whitePawnCount = 0, blackPawnCount = 0;

            for (int i = 0; i < squares.GetLength(0); i++)
            {
                for (int j = 0; j < squares.GetLength(1); j++)
                {
                    if (squares[i, j].Piece != null)
                    {
                        Piece piece = squares[i, j].Piece;
                        if (piece is King)
                        {
                            if (piece.Color == "White") whiteKingCount++; else blackKingCount++;
                        }
                        else if (piece is Rook)
                        {
                            if (piece.Color == "White") whiteRookCount++; else blackRookCount++;
                        }
                        else if (piece is Bishop)
                        {
                            if (piece.Color == "White") whiteBishopCount++; else blackBishopCount++;
                        }
                        else if (piece is Knight)
                        {
                            if (piece.Color == "White") whiteKnightCount++; else blackKnightCount++;
                        }
                        else if (piece is Queen)
                        {
                            if (piece.Color == "White") whiteQueenCount++; else blackQueenCount++;
                        }
                        else
                        {
                            if (piece.Color == "White") whitePawnCount++; else blackPawnCount++;
                        }
                    }
                }
            }

            if (whiteKingCount != 1 || blackKingCount != 1 || whiteBishopCount > 2 || blackBishopCount > 2 || whiteRookCount > 2 || blackRookCount > 2 || whiteKnightCount > 2 || blackKnightCount > 2 || whitePawnCount > 8 || blackPawnCount > 8 || whiteQueenCount > 1 || blackQueenCount > 1)
            {
                return false;
            }

            if (!AreTherePossibleMoves(squares, "White") || !AreTherePossibleMoves(squares, "Black"))
            {
                return false;
            }

            if (IsKingInCheck(squares, "White") || IsKingInCheck(squares, "Black"))
            {
                return false;
            }

            return true;
        }
    }
}
