using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
  
    public partial class ChessForm : Form
    {

        private Button[,] boardButtons;
        private Game game;
        private Label turnLabel;
        private Label timerLabel;
        private ComboBox TimeComboBox { get; set; }
        private const int BUTTON_SIZE = 60;
        private const int BOARD_MARGIN = 20;

        private Panel boardPanel;


        //flashing red chess:
        private Timer checkFlashTimer;
        private bool isFlashing;
        private Square checkSquare;

        

        public ChessForm()
        {
            InitializeComponent();

            int width = (BUTTON_SIZE*4) + (BOARD_MARGIN*2) + 150;
            int height = (BUTTON_SIZE * 8) + (BOARD_MARGIN * 3) + 60;

            this.Size = new Size(width, height);
            this.MinimumSize = this.Size;

            this.Size = new Size(BUTTON_SIZE * 6, BUTTON_SIZE * 9);
            InitializeComponents();

            InitializeCheckFlashTimer();

        }

        private void InitializeComponents()
        {

            TimeComboBox = new ComboBox
            {
                Location = new Point(BOARD_MARGIN + 300, BOARD_MARGIN),
                Size = new Size(80, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            TimeComboBox.Items.AddRange(new object[] { 20, 40, 60});
            TimeComboBox.SelectedIndex = 0;
            TimeComboBox.SelectionChangeCommitted += TimeComboBox_SelectionChanged;
            this.Controls.Add(TimeComboBox);

            Label timeSelectLabel = new Label
            {
                Text = "Time Limit:",
                Location = new Point(BOARD_MARGIN + 300, BOARD_MARGIN - 20),
                Size = new Size(80, 20),
                Font = new Font("Arial", 9)
            };
            this.Controls.Add(timeSelectLabel);

            turnLabel = new Label
            {
                Location = new Point(BOARD_MARGIN, BOARD_MARGIN),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Turn: White"
            };
            this.Controls.Add(turnLabel);


            timerLabel = new Label
            {
                Location = new Point(BOARD_MARGIN + 200, BOARD_MARGIN),
                Size = new Size(100, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Time: 20"
            };
            this.Controls.Add(timerLabel);

            boardPanel = new Panel  // Store the panel ref
            {
                Location = new Point(BOARD_MARGIN, BOARD_MARGIN + 40),
                Size = new Size(BUTTON_SIZE * 4, BUTTON_SIZE * 8),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(boardPanel);


            InitializeBoard();

        }




        private void InitializeCheckFlashTimer()
        {
            checkFlashTimer = new System.Windows.Forms.Timer();
            checkFlashTimer.Interval = 250;// 1/4 sec
            checkFlashTimer.Tick += CheckFlash_Tick;
        }


        private void CheckFlash_Tick(object sender, EventArgs e)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => CheckFlash_Tick(sender, e)));
                return;
            }


            if (isFlashing && checkSquare != null)
            {

                try
                {
                    //toggle the aklternating red color
                    Color newColor = boardButtons[checkSquare.Row, checkSquare.Col].BackColor == Color.Red ?
                        ((checkSquare.Row + checkSquare.Col) % 2 == 0 ? Color.SaddleBrown : Color.RosyBrown) : Color.Red;

                    boardButtons[checkSquare.Row, checkSquare.Col].BackColor = newColor;
                }

                catch(Exception ex)
                {
                    MessageBox.Show($"Error in flash ticking: {ex.Message}");
                    checkFlashTimer.Stop();
                    isFlashing = false;
                }
            }
        }

        private void TimeComboBox_SelectionChanged(Object sender, EventArgs e)
        {
            int selectedTime = (int)TimeComboBox.SelectedItem;
            game.SetTimeLimit(selectedTime);
        }

        public void DisableTimeSelection()
        {
            TimeComboBox.Enabled = false;
        }


        private void InitializeBoard()
        {
            boardButtons = new Button[8, 4];
        
            for(int row = 0; row<Board.Rows; row++)
            {
                for(int col = 0; col<Board.Columns; col++)
                {
                    boardButtons[row, col] = new Button
                    {
                        Size = new Size(BUTTON_SIZE, BUTTON_SIZE),
                        Location = new Point(col * BUTTON_SIZE, row * BUTTON_SIZE),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    boardButtons[row, col].Click += Square_Click;
                    boardPanel.Controls.Add(boardButtons[row, col]);
                }
            }


            ResetBoardColors();
            game = new Game(UpdateBoardUI);

            UpdateBoardUI(game.board, true, 20, false); // Initial board update
            //todo, super important UpdateBoardDisplay();

        }

        public void UpdateBoardUI(Board board, bool isWhiteTurn, int timeLeft, bool isCheck)
        {

            if (board == null)
            {
                MessageBox.Show("Board is null!");
                return;
            }


            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateBoardUI(board, isWhiteTurn, timeLeft, isCheck)));
                return;
            }


            try
            {
                // Update timer
                timerLabel.Text = $"Time: {timeLeft}";

                // Update turn label
                turnLabel.Text = $"Turn: {(isWhiteTurn ? "White" : "Black")}";

                // Update pieces
                for (int row = 0; row < Board.Rows; row++)
                {
                    for (int col = 0; col < Board.Columns; col++)
                    {
                        Piece piece = board.GetPiece(new Square(row, col));

                        if (piece == null)
                        {
                            MessageBox.Show($"Null piece at {row},{col}");
                            continue;
                        }


                        boardButtons[row, col].Text = GetPieceSymbol(piece);
                        boardButtons[row, col].ForeColor = piece.IsWhite ? Color.White : Color.Black;
                    }
                }

                // If in check, highlight the king
                if (isCheck)
                {
                    King kingInCheck = isWhiteTurn ? board.whiteKing : board.blackKing;


                    if (kingInCheck != null && kingInCheck.Position != null)
                    {

                        //handle flashing
                        checkSquare = kingInCheck.Position;
                        isFlashing = true;

                        if (checkFlashTimer != null && !checkFlashTimer.Enabled)
                        {
                            checkFlashTimer.Start();
                        }


                        //boardButtons[kingInCheck.Position.Row, kingInCheck.Position.Col].BackColor = Color.Red;
                    }
                }
                else
                {
                    isFlashing = false;

                    if (checkFlashTimer != null && checkFlashTimer.Enabled)
                    {
                        checkFlashTimer.Stop();
                    }

                    ResetBoardColors();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error updating UI: {ex.Message}");
            }


            
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (checkFlashTimer != null)
            {
                checkFlashTimer.Stop();
                checkFlashTimer.Dispose();
            }
            base.OnFormClosing(e);
        }


        public void HighlightSquare(Square position, Color color)
        {
            // Ensure position is valid
            if (position.Row >= 0 && position.Row < Board.Rows &&
                position.Col >= 0 && position.Col < Board.Columns)
            {
                boardButtons[position.Row, position.Col].BackColor = color;
            }
        }


        // Method to show valid moves by highlighting them
        public void ShowValidMoves(List<Square> validMoves)
        {
            foreach (Square move in validMoves)
            {
                HighlightSquare(move, Color.LightGreen);
            }
        }


        private void Square_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int row = clickedButton.Location.Y / BUTTON_SIZE;
            int col = clickedButton.Location.X / BUTTON_SIZE;

            game.HandleSquareClick(new Square(row, col), this);
        }

        public void ResetBoardColors()
        {
            for (int row = 0; row<Board.Rows; row++)
            {
                for(int col = 0; col<Board.Columns; col++)
                {
                    if ((row + col) %2 == 0)
                    {
                        boardButtons[row, col].BackColor = Color.SaddleBrown;
                        boardButtons[row, col].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        boardButtons[row, col].BackColor = Color.RosyBrown;
                        boardButtons[row, col].FlatAppearance.BorderColor = Color.Black;
                    }
                }
            }
        }


       

        private string GetPieceSymbol(Piece piece)
        {
            if (piece == null || piece.IsEmpty) return "";

            switch (piece.Type)
            {
                case PieceType.King: return "♔";
                case PieceType.Rook: return "♖";
                case PieceType.Bishop: return "♗";
                case PieceType.Knight: return "♘";
                case PieceType.Pawn: return "♙";
                default: return "";
            }
        }

    }
}
