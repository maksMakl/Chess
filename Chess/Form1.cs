using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        private Chessboard board;
        private Label turn;
        private Button pvpButton;
        private Button pvcButton;
        private Button clrButton;
        private Button saveButton;
        private Button loadButton;
        private InstrumentPanel instrumentsTop;
        private InstrumentPanel instrumentsBottom;

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(800, 600);
            this.Text = "Chess";

            board = new Chessboard();
        }

        #region MouseClick event handlers
        private void BoardOnClickPreGame(object sender, MouseEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            int i = (box.Location.Y - board.Dy) / board.SquareSideLen;
            int j = (box.Location.X - board.Dx) / board.SquareSideLen;
            ChessboardSquare square = board.squares[i, j];

            if (InstrumentPanel.selectedInstrument != null)
            {
                if (InstrumentPanel.selectedInstrument.Piece != null)
                {
                    square.SetPiece(InstrumentPanel.selectedInstrument.Piece.Clone() as Piece);
                }
                else
                {
                    square.SetDefault();
                }
            }
        }

        private void BoardOnClickInGamePVP(object sender, MouseEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            int i = (box.Location.Y - board.Dy) / board.SquareSideLen;
            int j = (box.Location.X - board.Dx) / board.SquareSideLen;
            ChessboardSquare square = board.squares[i, j];

            board.OnClick(i, j);
            turn.Text = String.Concat("Turn: ", board.Turn);
        }

        private void BoardOnClickInGamePVC(object sender, MouseEventArgs e)
        {
            if (board.Turn == "White")
            {
                PictureBox box = sender as PictureBox;
                int i = (box.Location.Y - board.Dy) / board.SquareSideLen;
                int j = (box.Location.X - board.Dx) / board.SquareSideLen;
                ChessboardSquare square = board.squares[i, j];

                board.OnClick(i, j);
                turn.Text = String.Concat("Turn: ", board.Turn);
            }

            if (board.Turn == "Black")
            {
                int[] aiMove = AI.Minimax(board.squares, board.Turn);
                board.squares[aiMove[2], aiMove[3]].SetPiece(board.squares[aiMove[0], aiMove[1]].Piece);
                board.squares[aiMove[0], aiMove[1]].SetDefault();
                board.Turn = "White";
                turn.Text = String.Concat("Turn: ", board.Turn);
                board.CheckGameEnding();
            }
        }

        private void PvpButtonOnClick(object sender, MouseEventArgs e)
        {
            #region Validate board
            if (!board.IsValid())
            {
                MessageBox.Show("Invalid position");
                return;
            }
            #endregion

            #region Add turn
            turn = new Label();
            turn.Text = String.Concat("Turn: ", board.Turn);
            turn.Location = new Point(2 * Size.Width / 3, board.Dy);
            turn.AutoSize = true;
            turn.Font = new Font(FontFamily.GenericSerif, 24);
            Controls.Add(turn);
            #endregion

            #region Add "save position" button
            Size btnSize = new Size(200, 70);

            saveButton = new Button();
            saveButton.Text = "SAVE POSITION";
            saveButton.Location = new Point(2 * Size.Width / 3, board.Dy + 100);
            saveButton.Size = btnSize;
            saveButton.MouseClick += new MouseEventHandler(SaveButtonOnClick);
            Controls.Add(saveButton);
            #endregion

            #region Remove buttons
            Controls.Remove(pvpButton);
            Controls.Remove(pvcButton);
            Controls.Remove(clrButton);
            Controls.Remove(loadButton);
            #endregion

            #region Remove instrument panels
            for (int i = 0; i < 7; i++)
            {
                Controls.Remove(instrumentsTop.instruments[i].Box);
                Controls.Remove(instrumentsBottom.instruments[i].Box);
            }
            #endregion

            #region Change event handler on squares
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board.squares[i, j].Box.MouseClick -= new MouseEventHandler(BoardOnClickPreGame);
                    board.squares[i, j].Box.MouseClick += new MouseEventHandler(BoardOnClickInGamePVP);
                }
            }
            #endregion
        }

        private void PvcButtonOnClick(object sender, MouseEventArgs e)
        {
            #region Validate board
            if (!board.IsValid())
            {
                MessageBox.Show("Invalid position");
                return;
            }
            #endregion

            #region Add turn
            turn = new Label();
            turn.Text = String.Concat("Turn: ", board.Turn);
            turn.Location = new Point(2 * Size.Width / 3, board.Dy);
            turn.AutoSize = true;
            turn.Font = new Font(FontFamily.GenericSerif, 24);
            Controls.Add(turn);
            #endregion

            #region Add "save position" button
            Size btnSize = new Size(200, 70);

            saveButton = new Button();
            saveButton.Text = "SAVE POSITION";
            saveButton.Location = new Point(2 * Size.Width / 3, board.Dy + 100);
            saveButton.Size = btnSize;
            saveButton.MouseClick += new MouseEventHandler(SaveButtonOnClick);
            Controls.Add(saveButton);
            #endregion

            #region Remove buttons
            Controls.Remove(pvpButton);
            Controls.Remove(pvcButton);
            Controls.Remove(clrButton);
            Controls.Remove(loadButton);
            #endregion

            #region Remove instrument panels
            for (int i = 0; i < 7; i++)
            {
                Controls.Remove(instrumentsTop.instruments[i].Box);
                Controls.Remove(instrumentsBottom.instruments[i].Box);
            }
            #endregion

            #region Change event handler on squares
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board.squares[i, j].Box.MouseClick -= new MouseEventHandler(BoardOnClickPreGame);
                    board.squares[i, j].Box.MouseClick += new MouseEventHandler(BoardOnClickInGamePVC);
                }
            }
            #endregion
        }

        private void ClrButtonOnClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.squares[i, j].Piece != null)
                    {
                        board.squares[i, j].SetDefault();
                    }
                }
            }
        }

        private void SaveButtonOnClick(object sender, MouseEventArgs e)
        {
            using (BinaryWriter binWriter = new BinaryWriter(File.Open("position.bin", FileMode.Create)))
            {
                binWriter.Write(board.Turn);

                for (int i = 0; i < board.squares.GetLength(0); i++)
                {
                    for (int j = 0; j < board.squares.GetLength(1); j++)
                    {
                        Piece piece = board.squares[i, j].Piece;

                        binWriter.Write(piece == null ? "." : piece.Name);
                        binWriter.Write(piece == null ? "." : piece.Color);
                    }
                }
            }
        }

        private void LoadButtonOnClick(object sender, MouseEventArgs e)
        {
            if (!File.Exists("position.bin"))
            {
                MessageBox.Show("File doesn't exits");
                return;
            }

            using (BinaryReader binReader = new BinaryReader(File.Open("position.bin", FileMode.Open)))
            {
                Hashtable piecesWhite = new Hashtable()
                {
                    {"king",  new King("White")},
                    {"queen", new Queen("White")},
                    {"rook", new Rook("White")},
                    {"bishop", new Bishop("White")},
                    {"knight", new Knight("White")},
                    {"pawn", new Pawn("White")},
                };

                Hashtable piecesBlack = new Hashtable()
                {
                    {"king",  new King("Black")},
                    {"queen", new Queen("Black")},
                    {"rook", new Rook("Black")},
                    {"bishop", new Bishop("Black")},
                    {"knight", new Knight("Black")},
                    {"pawn", new Pawn("Black")},
                };

                board.Turn = binReader.ReadString();
                for (int i = 0; i < board.squares.GetLength(0); i++)
                {
                    for (int j = 0; j < board.squares.GetLength(1); j++)
                    {
                        string pieceName = binReader.ReadString();
                        string pieceColor = binReader.ReadString();

                        if (pieceName == ".")
                        {
                            board.squares[i, j].SetDefault();
                        }
                        else if (pieceColor == "White")
                        {
                            Piece piece = piecesWhite[pieceName] as Piece;
                            board.squares[i, j].SetPiece(piece.Clone() as Piece);
                        }
                        else
                        {
                            Piece piece = piecesBlack[pieceName] as Piece;
                            board.squares[i, j].SetPiece(piece.Clone() as Piece);
                        }
                    }
                }
            }
        }

        private void InstrumentPanelTopOnClick(object sender, MouseEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            int i = (box.Location.X - instrumentsTop.Dx) / instrumentsTop.SquareLen;
            instrumentsTop.OnClick(i);
        }

        private void InstrumentPanelBottomOnClick(object sender, MouseEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            int i = (box.Location.X - instrumentsBottom.Dx) / instrumentsBottom.SquareLen;
            instrumentsBottom.OnClick(i);
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Draw Board
            board.SquareSideLen = Math.Min(Size.Width, Size.Height) / 12;
            board.Dx = (2 * Size.Width / 3 - board.SquareSideLen * 8) / 2;
            board.Dy = (Size.Height - board.SquareSideLen * 8) / 2 - 20;

            for (int i = 8; i >= 1; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    string pos = String.Concat(Convert.ToChar('a' + j), Convert.ToChar('0' + i));
                    try
                    {
                        board.squares[8 - i, j] = new ChessboardSquare(board.Dx, board.Dy, board.SquareSideLen, pos);
                    }
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show(String.Format("Couldn't find sprite {0}", Environment.CurrentDirectory + ex.Message), "Error!");
                        Environment.Exit(1);
                    }
                    
                    board.squares[8 - i, j].Box.MouseClick += new MouseEventHandler(BoardOnClickPreGame);
                    Controls.Add(board.squares[8 - i, j].Box);
                }
            }
            #endregion

            #region Place Pieces
            try 
            {
                for (int j = 0; j < 8; j++)
                {
                    board.squares[1, j].SetPiece(new Pawn("Black"));
                    board.squares[6, j].SetPiece(new Pawn("White"));
                }
                board.squares[0, 0].SetPiece(new Rook("Black"));
                board.squares[7, 0].SetPiece(new Rook("White"));
                board.squares[0, 1].SetPiece(new Knight("Black"));
                board.squares[7, 1].SetPiece(new Knight("White"));
                board.squares[0, 2].SetPiece(new Bishop("Black"));
                board.squares[7, 2].SetPiece(new Bishop("White"));
                board.squares[0, 3].SetPiece(new Queen("Black"));
                board.squares[7, 3].SetPiece(new Queen("White"));
                board.squares[0, 4].SetPiece(new King("Black"));
                board.squares[7, 4].SetPiece(new King("White"));
                board.squares[0, 5].SetPiece(new Bishop("Black"));
                board.squares[7, 5].SetPiece(new Bishop("White"));
                board.squares[0, 6].SetPiece(new Knight("Black"));
                board.squares[7, 6].SetPiece(new Knight("White"));
                board.squares[0, 7].SetPiece(new Rook("Black"));
                board.squares[7, 7].SetPiece(new Rook("White"));
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(String.Format("Couldn't find sprite {0}", Environment.CurrentDirectory + ex.Message), "Error!");
                Environment.Exit(1);
            }
            
            #endregion

            #region Add buttons
            Size btnSize = new Size(200, 70);

            pvpButton = new Button();
            pvpButton.Text = "P1 VS P2";
            pvpButton.Location = new Point(2 * Size.Width / 3, board.Dy);
            pvpButton.Size = btnSize;
            pvpButton.MouseClick += new MouseEventHandler(PvpButtonOnClick);
            Controls.Add(pvpButton);

            pvcButton = new Button();
            pvcButton.Text = "P1 VS COMPUTER";
            pvcButton.Location = new Point(2 * Size.Width / 3, board.Dy + 100);
            pvcButton.Size = btnSize;
            pvcButton.MouseClick += new MouseEventHandler(PvcButtonOnClick);
            Controls.Add(pvcButton);

            clrButton = new Button();
            clrButton.Text = "CLEAR BOARD";
            clrButton.Location = new Point(2 * Size.Width / 3, board.Dy + 200);
            clrButton.Size = btnSize;
            clrButton.MouseClick += new MouseEventHandler(ClrButtonOnClick);
            Controls.Add(clrButton);

            loadButton = new Button();
            loadButton.Text = "LOAD POSITION";
            loadButton.Location = new Point(2 * Size.Width / 3, board.Dy + 300);
            loadButton.Size = btnSize;
            loadButton.MouseClick += new MouseEventHandler(LoadButtonOnClick);
            Controls.Add(loadButton);
            #endregion

            #region Draw instrument panels
            int pnlSqrSideLen = Math.Min(Size.Width, Size.Height) / 14;
            instrumentsTop = new InstrumentPanel(board.Dx + (8 * board.SquareSideLen - 7 * pnlSqrSideLen) / 2, board.Dy - pnlSqrSideLen, pnlSqrSideLen, "Black");
            instrumentsBottom = new InstrumentPanel(board.Dx + (8 * board.SquareSideLen - 7 * pnlSqrSideLen) / 2, board.Dy + 8 * board.SquareSideLen, pnlSqrSideLen, "White");

            for (int i = 0; i < 7; i++)
            {
                instrumentsTop.instruments[i].Box.MouseClick += new MouseEventHandler(InstrumentPanelTopOnClick);
                Controls.Add(instrumentsTop.instruments[i].Box);
            }

            for (int i = 0; i < 7; i++)
            {
                instrumentsBottom.instruments[i].Box.MouseClick += new MouseEventHandler(InstrumentPanelBottomOnClick);
                Controls.Add(instrumentsBottom.instruments[i].Box);
            }
            #endregion
        }
    }
}
