using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class InstrumentPanelSquare
    {
        private PictureBox picture;
        private string instrument;

        public PictureBox Box { get { return picture; } }
        public Piece Piece { get; set; }

        public InstrumentPanelSquare(int x, int y, int sideLen, string instrument, string color)
        {
            this.instrument = instrument;

            switch (instrument)
            {
                case "king":
                    Piece = new King(color);
                    break;
                case "queen":
                    Piece = new Queen(color);
                    break;
                case "rook":
                    Piece = new Rook(color);
                    break;
                case "bishop":
                    Piece = new Bishop(color);
                    break;
                case "knight":
                    Piece = new Knight(color);
                    break;
                case "pawn":
                    Piece = new Pawn(color);
                    break;
                case "delete":
                    Piece = null;
                    break;
                default:
                    throw new ArgumentException();
            }

            picture = new PictureBox();
            picture.Size = new Size(sideLen, sideLen);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.Location = new Point(x, y);
            MakeNormal(); 
        }

        public void MakeSelected()
        {
            string path;
            if (instrument != "delete")
            {
                path = String.Format("Sprites\\Instruments\\{0}_Pieces_Selected\\{1}.png", Piece.Color, instrument);
            }
            else
            {
                path = "Sprites\\Instruments\\delete_selected.png";
            }
            picture.Image = Image.FromFile(path);
        }

        public void MakeNormal()
        {
            string path;
            if (instrument != "delete")
            {
                path = String.Format("Sprites\\Instruments\\{0}_Pieces_Normal\\{1}.png", Piece.Color, instrument);
            }
            else
            {
                path = "Sprites\\Instruments\\delete_normal.png";
            }
            picture.Image = Image.FromFile(path);
        }
    }
}
