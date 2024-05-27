using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public class ChessboardSquare
    {
        private readonly string position;
        private PictureBox picture;
        private int whiteSeenCount;
        private int blackSeenCount;

        public string Position { get { return position; } }
        public PictureBox Box { get { return picture; } }
        public bool Marked { get; set; }
        public Piece Piece { get; set; }
        public string SquareColor 
        {
            get { return (position[0] - 'a') % 2 == ('8' - position[1]) % 2 ? "White" : "Black"; }    
        }
        public bool SeenByWhite { get { return whiteSeenCount > 0; } }
        public bool SeenByBlack { get { return blackSeenCount > 0; } }

        public ChessboardSquare(int dx, int dy, int sideLen, string position)
        {
            int x = dx + sideLen * (position[0] - 'a');
            int y = dy + sideLen * (8 - (position[1] - '0'));

            this.position = position;

            picture = new PictureBox();
            picture.Size = new Size(sideLen, sideLen);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.Location = new Point(x, y);

            whiteSeenCount = 0;
            blackSeenCount = 0;

            SetDefault();
        }

        public void SetDefault()
        {
            string path = String.Format("Sprites\\Squares\\{0}\\default.png", SquareColor);
            picture.Image = Image.FromFile(path);
            Marked = false;
            Piece = null;
        }

        public void Mark()
        {
            string path = String.Format("Sprites\\Squares\\{0}\\marked.png", SquareColor);
            picture.Image = Image.FromFile(path);
            Marked = true;
        }

        public void MarkCapture()
        {
            picture.BorderStyle = BorderStyle.FixedSingle;
            Marked = true;
        }

        public void UnmarkCapture()
        {
            picture.BorderStyle = BorderStyle.None;
            Marked = false;
        }

        public void SetPiece(Piece piece)
        {
            if (Position[1] == '8' && piece is Pawn && piece.Color == "White")
            {
                string path = String.Format("Sprites\\White_Pieces\\{0}_Square\\queen.png", SquareColor);
                picture.Image = Image.FromFile(path);
                Piece = new Queen("White");
            }
            else if (Position[1] == '1' && piece is Pawn && piece.Color == "Black")
            {
                string path = String.Format("Sprites\\Black_Pieces\\{0}_Square\\queen.png", SquareColor);
                picture.Image = Image.FromFile(path);
                Piece = new Queen("Black");
            }
            else
            {
                string path = String.Format("Sprites\\{0}_Pieces\\{1}_Square\\{2}.png", piece.Color, SquareColor, piece.Name);
                picture.Image = Image.FromFile(path);
                Piece = piece;
            }
        }

        public void IncreaseWhiteSeenCount()
        {
            whiteSeenCount++;
        }

        public void IncreaseBlackSeenCount() 
        { 
            blackSeenCount++;
        }

        public void ResetSeenCount()
        {
            whiteSeenCount = 0;
            blackSeenCount = 0;
        }
    }
}
