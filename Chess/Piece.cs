using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class Piece : ICloneable
    {
        public static int[] flip = 
        {
            56,  57,  58,  59,  60,  61,  62,  63,
            48,  49,  50,  51,  52,  53,  54,  55,
            40,  41,  42,  43,  44,  45,  46,  47,
            32,  33,  34,  35,  36,  37,  38,  39,
            24,  25,  26,  27,  28,  29,  30,  31,
            16,  17,  18,  19,  20,  21,  22,  23,
            8,   9,  10,  11,  12,  13,  14,  15,
            0,   1,   2,   3,   4,   5,   6,   7
        };

        public int Value { get; protected set; }
        public string Color { get; protected set; }
        public string Name { get; protected set; }

        public bool HasMoved { get; set; }
        public abstract Tuple<int, int>[] PossibleMoves(int i, int j, ChessboardSquare[,] squares);


        public void MarkSquares(int i, int j, ChessboardSquare[,] squares)
        {
            foreach (Tuple<int, int> move in PossibleMoves(i, j, squares))
            {
                ChessboardSquare square = squares[move.Item1, move.Item2];

                if (square.Piece != null)
                {
                    square.MarkCapture();
                }
                else
                {
                    square.Mark();
                }
            }
        }
        public void UnmarkSquares(int i, int j, ChessboardSquare[,] squares)
        {
            foreach (Tuple<int, int> move in PossibleMoves(i, j, squares))
            {
                ChessboardSquare square = squares[move.Item1, move.Item2];

                if (square.Piece != null)
                {
                    square.UnmarkCapture();
                }
                else
                {
                    square.SetDefault();
                }
            }
        }
        public abstract void MarkSeen(int i, int j, ChessboardSquare[,] squares);

        public object Clone()
        {
            if (this is King)
            {
                return new King(Color);
            } 
            else if (this is Queen)
            {
                return new Queen(Color);
            } 
            else if (this is Rook)
            {
                return new Rook(Color);
            } 
            else if (this is Bishop)
            {
                return new Bishop(Color);
            } 
            else if (this is Knight)
            {
                return new Knight(Color);
            } 
            else 
            {
                return new Pawn(Color);
            }
        }
    }
}
