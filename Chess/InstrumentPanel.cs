using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class InstrumentPanel
    {
        public InstrumentPanelSquare[] instruments;
        public static InstrumentPanelSquare selectedInstrument;

        public int Dx { get; private set; }
        public int Dy { get; private set; }
        public int SquareLen { get; private set; }

        public InstrumentPanel(int dx, int dy, int sqrLen, string color) 
        { 
            this.instruments = new InstrumentPanelSquare[7];
            selectedInstrument = null;
            SquareLen = sqrLen;
            Dx = dx;
            Dy = dy;

            string[] instruments = { "king", "queen", "rook", "bishop", "knight", "pawn", "delete" };
            for (int i = 0; i < 7; i++)
            {
                int x = dx + sqrLen * i;
                int y = dy;
                this.instruments[i] = new InstrumentPanelSquare(x, y, sqrLen, instruments[i], color);
            }
        }

        public void OnClick(int i)
        {
            if (selectedInstrument != null)
            {
                selectedInstrument.MakeNormal();
            }

            selectedInstrument = instruments[i];
            selectedInstrument.MakeSelected();
        }
    }
}
