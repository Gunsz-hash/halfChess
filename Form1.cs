//using System;
//using System.CodeDom.Compiler;
//using System.Drawing;
//using System.Windows.Forms;

//namespace FinalProject
//{
//    public partial class Form1 : Form
//    {
//        private int x, y;
//        private Bitmap bitmap;
//        private Timer flashTimer;
//        private bool flashRed =false;
//        private Button[,] boardButtons = new Button[8, 4];
//        private string[,] board = new string[8, 4];
//        private bool isWhiteTurn = true;  // Track the turn (White starts first)
//        private Point whiteKingPosition;
//        private Point blackKingPosition;
//        private Point? selectedPiece = null;
//        private Timer moveTimer;
//        //itamar branch

//        private ComboBox intervalComboBox {  get; set; }    



//        public Form1()
//        {
//            InitializeComponent();
//            InitializeBoard();
//            PlacePieces();

//            // Adjust the form size to make it bigger and provide space for the ComboBox
//            this.ClientSize = new Size(5 * 120, 8 * 120 + 100); // Increase form height by 100 for ComboBox area

//            // Initialize ComboBox for interval selection
//            intervalComboBox = new ComboBox();

//            // Update interval choices to 20, 60, 90
//            intervalComboBox.Items.AddRange(new object[] { 20, 60, 90 });
//            intervalComboBox.SelectedItem = 20; // Set default to 20
//            intervalComboBox.Location = new Point(10, 350); // Adjust location as needed
//            intervalComboBox.SelectedIndexChanged += IntervalComboBox_SelectedIndexChanged;
//            Controls.Add(intervalComboBox);

//            // Initialize timer for move delays
//            moveTimer = new Timer();
//            moveTimer.Interval = 2000; // Default interval (20 seconds, set by default ComboBox value)
//            moveTimer.Tick += MoveTimer_Tick;
//        }

//        private Rectangle r = new Rectangle(30,30,200,200);







//        private void IntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
//        {

//        }
//        private void InitializeBoard()
//        {
//            int tileWidth = 100; // Wider tiles
//            int tileHeight = 80; // Adjust height to balance proportions
//            Font pieceFont = new Font("Segoe UI Symbol", 24, FontStyle.Bold); // Larger font for pieces

//            for (int row = 0; row < 8; row++)
//            {
//                for (int col = 0; col < 4; col++)
//                {
//                    var button = new Button();
//                    button.Size = new Size(tileWidth, tileHeight);
//                    button.Location = new Point(col * tileWidth, row * tileHeight);
//                    button.Font = pieceFont;
//                    button.Click += Button_Click;
//                    button.Tag = new Point(row, col); // Store position in the Tag

//                    // Color alternation
//                    button.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;

//                    Controls.Add(button);
//                    boardButtons[row, col] = button;
//                }
//            }
//        }
//        private void MoveTimer_Tick(object sender, EventArgs e)
//        {
//            // Perform any timed actions here, like updating the game state or piece animations.
//            // The moveTimer.Interval is now adjusted based on the ComboBox choice.
//        }
//        private void PlacePieces()
//        {
//            board[0, 0] = "♚"; blackKingPosition = new Point(0, 0); // Black King
//            board[0, 1] = "♝"; // Black Bishop
//            board[0, 2] = "♞"; // Black Knight
//            board[0, 3] = "♜"; // Black Rook
//            for (int i = 0; i < 4; i++) board[1, i] = "♟"; // Black pawns

//            for (int i = 0; i < 4; i++) board[6, i] = "♙"; // White pawns
//            board[7, 0] = "♔"; // White King
//            whiteKingPosition = new Point(7, 0);
//            board[7, 1] = "♗"; // White Bishop
//            board[7, 2] = "♘"; // White Knight
//            board[7, 3] = "♖"; // White Rook

//            UpdateBoard();
//        }

//        private void UpdateBoard()
//        {
//            for (int row = 0; row < 8; row++)
//            {
//                for (int col = 0; col < 4; col++)
//                {
//                    boardButtons[row, col].Text = board[row, col] ?? "";
//                }
//            }
//        }

//        private void Button_Click(object sender, EventArgs e)
//        {
//            var button = (Button)sender;
//            var position = (Point)button.Tag;
//            int row = position.X, col = position.Y;

//            // Check if the piece at the selected position belongs to the current player
//            bool isWhitePiece = board[row, col] != null && "♔♕♖♗♘♙".Contains(board[row, col]);
//            bool isBlackPiece = board[row, col] != null && "♚♛♜♝♞♟".Contains(board[row, col]);

//            // Prevent selecting an opponent's piece as the initial selection
//            if (selectedPiece == null && ((isWhiteTurn && isBlackPiece) || (!isWhiteTurn && isWhitePiece)))
//            {
//                MessageBox.Show("It is not your turn.");
//                return;
//            }

//            // Handle piece selection
//            if (selectedPiece == null)
//            {
//                // Select the piece only if it belongs to the current player
//                if (board[row, col] != null)
//                {
//                    selectedPiece = position;
//                    boardButtons[row, col].BackColor = Color.Yellow;
//                }
//            }
//            else
//            {
//                var selectedRow = selectedPiece.Value.X;
//                var selectedCol = selectedPiece.Value.Y;

//                // If the same piece is clicked again, deselect it
//                if (selectedRow == row && selectedCol == col)
//                {
//                    ResetBoardColors();
//                    selectedPiece = null;
//                    return;
//                }

//                // Determine if it's a move to an occupied square of the same side
//                bool isDestinationSameSide = (isWhiteTurn && isWhitePiece) || (!isWhiteTurn && isBlackPiece);
//                if (isDestinationSameSide)
//                {
//                    MessageBox.Show("Invalid move. You cannot move to a square occupied by your own piece.");
//                    return;
//                }

//                // Determine if move is a capture or a regular move
//                bool isOpponentPiece = (isWhiteTurn && isBlackPiece) || (!isWhiteTurn && isWhitePiece);

//                // If it's an opponent's piece, attempt a capture
//                if (isOpponentPiece && IsValidMove(selectedRow, selectedCol, row, col))
//                {
//                    PerformMove(selectedRow, selectedCol, row, col, capture: true);
//                }
//                else if (!isOpponentPiece && IsValidMove(selectedRow, selectedCol, row, col))
//                {
//                    PerformMove(selectedRow, selectedCol, row, col, capture: false);
//                }
//                else
//                {
//                    MessageBox.Show("Invalid move.");
//                }
//            }
//        }

//        private void CheckValidator(bool isWhiteTurn)
//        {

//        }

//        private bool IsKingCheckmated(bool isWhiteKing)
//        {
//            // Check if the king itself has no legal moves
//            Point kingPosition = isWhiteKing ? whiteKingPosition : blackKingPosition;
//            if (HasSafeKingMoves(kingPosition, isWhiteKing))
//            {
//                return false; // King has at least one move to avoid check
//            }

//            // Check if any other piece can block or capture the threatening piece
//            for (int row = 0; row < 8; row++)
//            {
//                for (int col = 0; col < 4; col++)
//                {
//                    if (board[row, col] != null && IsPieceColorCorrect(row, col, isWhiteKing))
//                    {
//                        Point startPosition = new Point(row, col);

//                        // Try moving this piece to every other position on the board
//                        for (int targetRow = 0; targetRow < 8; targetRow++)
//                        {
//                            for (int targetCol = 0; targetCol < 4; targetCol++)
//                            {
//                                if (IsValidMove(row, col, targetRow, targetCol))
//                                {
//                                    // Temporarily make the move
//                                    string tempEndPiece = board[targetRow, targetCol];
//                                    board[targetRow, targetCol] = board[row, col];
//                                    board[row, col] = null;

//                                    bool kingInCheckAfterMove = IsKingInCheck(isWhiteKing) != null;

//                                    // Revert the move
//                                    board[row, col] = board[targetRow, targetCol];
//                                    board[targetRow, targetCol] = tempEndPiece;

//                                    // If this move stops the king from being in check, it’s not checkmate
//                                    if (!kingInCheckAfterMove)
//                                    {
//                                        return false;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            // No valid moves to avoid check - king is checkmated
//            return true;
//        }


//        private void CanPreformMove(int startRow, int startCol, int endRow, int endCol, bool capture)
//        {
//            // Capture or move to the new position
//            string movingPiece = board[startRow, startCol];
//            board[endRow, endCol] = movingPiece;
//            board[startRow, startCol] = null;

//            // Update king position if moved
//            if (movingPiece == "♔") whiteKingPosition = new Point(endRow, endCol);
//            if (movingPiece == "♚") blackKingPosition = new Point(endRow, endCol);

//            // Reset selection, update turn, and refresh board UI
//            selectedPiece = null;
//            isWhiteTurn = !isWhiteTurn;
//            ResetBoardColors();
//            UpdateBoard();

//            // Check if the opponent's king is in checkmate after the move
//            //CheckForCheckmate();
//        }

//        private void PerformMove(int selectedRow, int selectedCol, int row, int col, bool capture)
//        {
//            var originalPiece = board[selectedRow, selectedCol];
//            var capturedPiece = board[row, col];
//            var originalPos = new Point(selectedRow, selectedCol);
//            var newPos = new Point(row, col);

//            // Make the move
//            board[row, col] = originalPiece;
//            board[selectedRow, selectedCol] = null;

//            // Update king position
//            if (originalPiece == "WK") whiteKingPosition = newPos;
//            else if (originalPiece == "BK") blackKingPosition = newPos;

//            // Validate move doesn't leave own king in check
//            if (IsKingInCheck(isWhiteTurn) != null)
//            {
//                // Revert move
//                board[selectedRow, selectedCol] = originalPiece;
//                board[row, col] = capturedPiece;

//                if (originalPiece == "WK") whiteKingPosition = originalPos;
//                else if (originalPiece == "BK") blackKingPosition = originalPos;

//                MessageBox.Show("Invalid move! You cannot leave your king in check.");
//                return;
//            }

//            // Move is valid, switch turns
//            isWhiteTurn = !isWhiteTurn;

//            // Handle capture notification
//            if (capture && capturedPiece != null)
//            {
//                MessageBox.Show($"Captured {capturedPiece}!");
//            }

//            // Check if opponent is now in check
//            var checkingPiece = CheckValidator(!isWhiteTurn);
//            if (checkingPiece != null)
//            {
//                var kingColor = isWhiteTurn ? "Black" : "White";
//                MessageBox.Show($"{kingColor} King is in check by {checkingPiece}!");
//            }

//            // Cleanup
//            selectedPiece = null;
//            ResetBoardColors();
//            UpdateBoard();
//        }
//        private bool HasSafeKingMoves(Point kingPosition, bool isWhiteKing)
//        {
//            int[] directions = { -1, 0, 1 };
//            foreach (int rowDir in directions)
//            {
//                foreach (int colDir in directions)
//                {
//                    if (rowDir == 0 && colDir == 0)
//                        continue;

//                    int newRow = kingPosition.X + rowDir;
//                    int newCol = kingPosition.Y + colDir;

//                    if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 4)
//                    {
//                        string originalPiece = board[newRow, newCol];

//                        // Temporarily move king
//                        board[kingPosition.X, kingPosition.Y] = null;
//                        board[newRow, newCol] = isWhiteKing ? "♔" : "♚";

//                        bool isSafe = IsKingInCheck(isWhiteKing) == null;

//                        // Revert king position
//                        board[kingPosition.X, kingPosition.Y] = isWhiteKing ? "♔" : "♚";
//                        board[newRow, newCol] = originalPiece;

//                        if (isSafe)
//                        {
//                            return true;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        //private bool IsPieceWhite(int row, int col)
//        //{
//        //    // Check if the piece at the given position is white (White pieces: ♔, ♗, ♘, ♖, ♙)
//        //    return board[row, col] != null && "♔♗♘♖♙".Contains(board[row, col]);
//        //}

//        //private bool CheckForCheckmate()
//        //{
//        //    if (


//        //}


//        private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
//        {

//            string piece = board[startRow, startCol];
//            bool isWhitePiece = "♔♗♘♖♙".Contains(piece); // White pieces
//            bool isBlackPiece = "♚♝♞♜♟".Contains(piece); // Black pieces

//            if (piece == null)
//            {
//                return false; // No piece to move
//            }

//            if (isWhiteTurn && isBlackPiece || !isWhiteTurn && isWhitePiece)
//            {
//                return false; // Wrong turn
//            }

//            switch (piece)
//            {
//                case "WP": // White Pawn
//                    if (startCol == endCol)
//                    {
//                        // Single square move
//                        if (endRow == startRow - 1 && board[endRow, endCol] == null)
//                            return true;

//                        // First move: two-square move
//                        if (startRow == 6 && endRow == startRow - 2 && board[endRow, endCol] == null && board[startRow - 1, startCol] == null)
//                            return true;
//                    }

//                    // Capture
//                    if (Math.Abs(startCol - endCol) == 1 && endRow == startRow - 1 && board[endRow, endCol] != null)
//                        return true;

//                    break;

//                case "BP": // Black Pawn
//                    if (startCol == endCol)
//                    {
//                        // Single square move
//                        if (endRow == startRow + 1 && board[endRow, endCol] == null)
//                            return true;

//                        // First move: two-square move
//                        if (startRow == 1 && endRow == startRow + 2 && board[endRow, endCol] == null && board[startRow + 1, startCol] == null)
//                            return true;
//                    }

//                    // Capture
//                    if (Math.Abs(startCol - endCol) == 1 && endRow == startRow + 1 && board[endRow, endCol] != null)
//                        return true;

//                    break;

//                case "WN": // White Knight
//                case "BN": // Black Knight
//                    if (Math.Abs(startRow - endRow) == 2 && Math.Abs(startCol - endCol) == 1 ||
//                        Math.Abs(startRow - endRow) == 1 && Math.Abs(startCol - endCol) == 2)
//                        return true;
//                    break;

//                case "WB": // White Bishop
//                case "BB": // Black Bishop
//                    if (Math.Abs(startRow - endRow) == Math.Abs(startCol - endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
//                        return true;
//                    break;

//                case "WR": // White Rook
//                case "BR": // Black Rook
//                    if ((startRow == endRow || startCol == endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
//                        return true;
//                    break;



//                case "WK": // White King
//                case "BK": // Black King
//                    if (Math.Abs(startRow - endRow) <= 1 && Math.Abs(startCol - endCol) <= 1)
//                        return true;
//                    break;
//            }

//            return false;
//        }

//        private bool IsPathClear(Point start, Point end)
//        {
//            int rowDiff = Math.Sign(end.X - start.X);
//            int colDiff = Math.Sign(end.Y - start.Y);

//            int row = start.X + rowDiff;
//            int col = start.Y + colDiff;

//            // Check if path is clear for sliding pieces (Bishop, Rook, Queen)
//            while (row != end.X || col != end.Y)
//            {
//                if (board[row, col] != null)
//                {
//                    return false; // There is a piece blocking the path
//                }
//                row += rowDiff;
//                col += colDiff;
//            }
//            return true;
//        }

//        private void Checkmate()
//        {

//                // Stop the game and declare the winner
//                string winner = isWhiteTurn ? "Black" : "White";
//                MessageBox.Show($"{winner} wins! The opponent's king is checkmated.");
//                Application.Exit(); // Or reset the board for a new game, if desired

//        }

//        private string IsKingInCheck(bool checkWhiteKing)
//        {
//            Point kingPosition = checkWhiteKing ? whiteKingPosition : blackKingPosition;
//            string checkingPiece = null;

//            // Loop through all board positions to find pieces of the opposite color
//            for (int row = 0; row < 8; row++)
//            {
//                for (int col = 0; col < 4; col++)
//                {
//                    if (board[row, col] != null && IsPieceColorCorrect(row, col, !checkWhiteKing)) // Check opposite color
//                    {
//                        string piece = board[row, col];

//                        // Check if this piece can attack the king's position
//                        if ((piece == "♖" || piece == "♜" ) && (row == kingPosition.X || col == kingPosition.Y))
//                        {
//                            if (IsPathClear(kingPosition, new Point(row, col)))
//                            {
//                                checkingPiece = piece;
//                                //CheckForCheckmate();

//                                return checkingPiece;
//                            }
//                        }
//                        if ((piece == "♗" || piece == "♝" ) && Math.Abs(row - kingPosition.X) == Math.Abs(col - kingPosition.Y))
//                        {
//                            if (IsPathClear(kingPosition, new Point(row, col)))
//                            {
//                                checkingPiece = piece;
//                                //CheckForCheckmate();

//                                return checkingPiece;
//                            }
//                        }
//                        if ((piece == "♘" || piece == "♞") && IsKnightAttack(kingPosition, new Point(row, col)))
//                        {
//                            checkingPiece = piece;
//                            //CheckForCheckmate();
//                            return checkingPiece;
//                        }
//                    }
//                }
//            }
//            return checkingPiece;
//        }

//        private bool IsKnightAttack(Point kingPosition, Point piecePosition)
//        {
//            int rowDiff = Math.Abs(kingPosition.X - piecePosition.X);
//            int colDiff = Math.Abs(kingPosition.Y - piecePosition.Y);
//            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
//        }


//        private bool IsPieceColorCorrect(int row, int col, bool checkWhite)
//        {
//            if (board[row, col] == null)
//                return false;

//            return checkWhite ? "♔♗♘♖♙".Contains(board[row, col]) : "♚♝♞♜♟".Contains(board[row, col]);
//        }
//        private void ResetBoardColors()
//        {
//            for (int row = 0; row < 8; row++)
//            {
//                for (int col = 0; col < 4; col++)
//                {
//                    boardButtons[row, col].BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
//                }
//            }
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            //this.DoubleBuffered = true;
//            bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);

//        }


//        private void Form1_Paint(object sender, PaintEventArgs e)
//        {
//            const int SIZE = 5;
//            Graphics g = Graphics.FromImage(bitmap);
//            g.FillEllipse(Brushes.AliceBlue, x, y, SIZE, SIZE);
//            e.Graphics.DrawImage(bitmap, x, y);

//            g.Dispose();
//        }
//        private void Form1_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                x = e.X;
//                y = e.Y;
//                this.Invalidate();  // Trigger a repaint
//            }
//        }
//        private void Form1_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left)
//            {
//                x = e.X;
//                y = e.Y;
//                this.Invalidate();
//                //this.Update();
//            }



//        }
//    }
//}

using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Form1 : Form
    {
        private int x, y;
        private Bitmap bitmap;
        private Timer flashTimer;
        private bool flashRed = false;
        private Button[,] boardButtons = new Button[8, 4];
        private string[,] board = new string[8, 4];
        private bool isWhiteTurn = true;  // Track the turn (White starts first)
        private Point whiteKingPosition;
        private Point blackKingPosition;
        private Point? selectedPiece = null;
        private Timer moveTimer;

        private ComboBox intervalComboBox { get; set; }

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            PlacePieces();

            this.ClientSize = new Size(5 * 120, 8 * 120 + 100);

            intervalComboBox = new ComboBox();
            intervalComboBox.Items.AddRange(new object[] { 20, 60, 90 });
            intervalComboBox.SelectedItem = 20;
            intervalComboBox.Location = new Point(10, 350);
            intervalComboBox.SelectedIndexChanged += IntervalComboBox_SelectedIndexChanged;
            Controls.Add(intervalComboBox);

            moveTimer = new Timer();
            moveTimer.Interval = 2000;
            moveTimer.Tick += MoveTimer_Tick;
        }

        private Rectangle r = new Rectangle(30, 30, 200, 200);

        private void IntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void InitializeBoard()
        {
            int tileWidth = 100;
            int tileHeight = 80;
            Font pieceFont = new Font("Segoe UI Symbol", 24, FontStyle.Bold);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    var button = new Button();
                    button.Size = new Size(tileWidth, tileHeight);
                    button.Location = new Point(col * tileWidth, row * tileHeight);
                    button.Font = pieceFont;
                    button.Click += Button_Click;
                    button.Tag = new Point(row, col);
                    button.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                    Controls.Add(button);
                    boardButtons[row, col] = button;
                }
            }
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
        }

        private void PlacePieces()
        {
            board[0, 0] = "BK"; blackKingPosition = new Point(0, 0);
            board[0, 1] = "BB";
            board[0, 2] = "BN";
            board[0, 3] = "BR";
            for (int i = 0; i < 4; i++) board[1, i] = "BP";

            for (int i = 0; i < 4; i++) board[6, i] = "WP";
            board[7, 0] = "WK";
            whiteKingPosition = new Point(7, 0);
            board[7, 1] = "WB";
            board[7, 2] = "WN";
            board[7, 3] = "WR";

            UpdateBoard();
        }

        private void UpdateBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    boardButtons[row, col].Text = board[row, col] ?? "";
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var position = (Point)button.Tag;
            int row = position.X, col = position.Y;

            bool isWhitePiece = board[row, col] != null && "WK,WB,WN,WR,WP".Contains(board[row, col]);
            bool isBlackPiece = board[row, col] != null && "BK,BB,BN,BR,BP".Contains(board[row, col]);

            if (selectedPiece == null && ((isWhiteTurn && isBlackPiece) || (!isWhiteTurn && isWhitePiece)))
            {
                MessageBox.Show("It is not your turn.");
                return;
            }

            if (selectedPiece == null)
            {
                if (board[row, col] != null)
                {
                    selectedPiece = position;
                    boardButtons[row, col].BackColor = Color.Yellow;
                }
            }
            else
            {
                var selectedRow = selectedPiece.Value.X;
                var selectedCol = selectedPiece.Value.Y;

                if (selectedRow == row && selectedCol == col)
                {
                    ResetBoardColors();
                    selectedPiece = null;
                    return;
                }

                bool isDestinationSameSide = (isWhiteTurn && isWhitePiece) || (!isWhiteTurn && isBlackPiece);
                if (isDestinationSameSide)
                {
                    MessageBox.Show("Invalid move. You cannot move to a square occupied by your own piece.");
                    return;
                }

                bool isOpponentPiece = (isWhiteTurn && isBlackPiece) || (!isWhiteTurn && isWhitePiece);

                if (isOpponentPiece && IsValidMove(selectedRow, selectedCol, row, col))
                {
                    PerformMove(selectedRow, selectedCol, row, col, capture: true);
                }
                else if (!isOpponentPiece && IsValidMove(selectedRow, selectedCol, row, col))
                {
                    PerformMove(selectedRow, selectedCol, row, col, capture: false);
                }
                else
                {
                    MessageBox.Show("Invalid move.");
                }
            }
        }

        private void CheckValidator(bool isWhiteTurn)
        {
        }

        private bool IsKingCheckmated(bool isWhiteKing)
        {
            Point kingPosition = isWhiteKing ? whiteKingPosition : blackKingPosition;
            if (HasSafeKingMoves(kingPosition, isWhiteKing))
            {
                return false;
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != null && IsPieceColorCorrect(row, col, isWhiteKing))
                    {
                        Point startPosition = new Point(row, col);

                        for (int targetRow = 0; targetRow < 8; targetRow++)
                        {
                            for (int targetCol = 0; targetCol < 4; targetCol++)
                            {
                                if (IsValidMove(row, col, targetRow, targetCol))
                                {
                                    string tempEndPiece = board[targetRow, targetCol];
                                    board[targetRow, targetCol] = board[row, col];
                                    board[row, col] = null;

                                    bool kingInCheckAfterMove = IsKingInCheck(isWhiteKing) != null;

                                    board[row, col] = board[targetRow, targetCol];
                                    board[targetRow, targetCol] = tempEndPiece;

                                    if (!kingInCheckAfterMove)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void CanPreformMove(int startRow, int startCol, int endRow, int endCol, bool capture)
        {
            string movingPiece = board[startRow, startCol];
            board[endRow, endCol] = movingPiece;
            board[startRow, startCol] = null;

            if (movingPiece == "WK") whiteKingPosition = new Point(endRow, endCol);
            if (movingPiece == "BK") blackKingPosition = new Point(endRow, endCol);

            selectedPiece = null;
            isWhiteTurn = !isWhiteTurn;
            ResetBoardColors();
            UpdateBoard();
        }

        private void PerformMove(int selectedRow, int selectedCol, int row, int col, bool capture)
        {
            var originalPiece = board[selectedRow, selectedCol];
            var capturedPiece = board[row, col];
            var originalPos = new Point(selectedRow, selectedCol);
            var newPos = new Point(row, col);

            board[row, col] = originalPiece;
            board[selectedRow, selectedCol] = null;

            if (originalPiece == "WK") whiteKingPosition = newPos;
            else if (originalPiece == "BK") blackKingPosition = newPos;

            if (IsKingInCheck(isWhiteTurn) != null)
            {
                board[selectedRow, selectedCol] = originalPiece;
                board[row, col] = capturedPiece;

                if (originalPiece == "WK") whiteKingPosition = originalPos;
                else if (originalPiece == "BK") blackKingPosition = originalPos;

                MessageBox.Show("Invalid move! You cannot leave your king in check.");
                return;
            }

            isWhiteTurn = !isWhiteTurn;

            if (capture && capturedPiece != null)
            {
                MessageBox.Show($"Captured {capturedPiece}!");
            }

            var checkingPiece = CheckValidator(!isWhiteTurn);
            if (checkingPiece != null)
            {
                var kingColor = isWhiteTurn ? "Black" : "White";
                MessageBox.Show($"{kingColor} King is in check by {checkingPiece}!");
            }

            selectedPiece = null;
            ResetBoardColors();
            UpdateBoard();
        }

        private bool HasSafeKingMoves(Point kingPosition, bool isWhiteKing)
        {
            int[] directions = { -1, 0, 1 };
            foreach (int rowDir in directions)
            {
                foreach (int colDir in directions)
                {
                    if (rowDir == 0 && colDir == 0)
                        continue;

                    int newRow = kingPosition.X + rowDir;
                    int newCol = kingPosition.Y + colDir;

                    if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 4)
                    {
                        string originalPiece = board[newRow, newCol];

                        board[kingPosition.X, kingPosition.Y] = null;
                        board[newRow, newCol] = isWhiteKing ? "WK" : "BK";

                        bool isSafe = IsKingInCheck(isWhiteKing) == null;

                        board[kingPosition.X, kingPosition.Y] = isWhiteKing ? "WK" : "BK";
                        board[newRow, newCol] = originalPiece;

                        if (isSafe)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
        {
            string piece = board[startRow, startCol];
            bool isWhitePiece = "WK,WB,WN,WR,WP".Contains(piece);
            bool isBlackPiece = "BK,BB,BN,BR,BP".Contains(piece);

            if (piece == null)
            {
                return false;
            }

            if (isWhiteTurn && isBlackPiece || !isWhiteTurn && isWhitePiece)
            {
                return false;
            }

            switch (piece)
            {
                case "WP":
                    if (startCol == endCol)
                    {
                        if (endRow == startRow - 1 && board[endRow, endCol] == null)
                            return true;

                        if (startRow == 6 && endRow == startRow - 2 && board[endRow, endCol] == null && board[startRow - 1, startCol] == null)
                            return true;
                    }

                    if (Math.Abs(startCol - endCol) == 1 && endRow == startRow - 1 && board[endRow, endCol] != null)
                        return true;
                    break;

                case "BP":
                    if (startCol == endCol)
                    {
                        if (endRow == startRow + 1 && board[endRow, endCol] == null)
                            return true;

                        if (startRow == 1 && endRow == startRow + 2 && board[endRow, endCol] == null && board[startRow + 1, startCol] == null)
                            return true;
                    }

                    if (Math.Abs(startCol - endCol) == 1 && endRow == startRow + 1 && board[endRow, endCol] != null)
                        return true;
                    break;

                case "WN":
                case "BN":
                    if (Math.Abs(startRow - endRow) == 2 && Math.Abs(startCol - endCol) == 1 ||
                        Math.Abs(startRow - endRow) == 1 && Math.Abs(startCol - endCol) == 2)
                        return true;
                    break;

                case "WB":
                case "BB":
                    if (Math.Abs(startRow - endRow) == Math.Abs(startCol - endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;

                case "WR":
                case "BR":
                    if ((startRow == endRow || startCol == endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;

                case "WK":
                case "BK":
                    if (Math.Abs(startRow - endRow) <= 1 && Math.Abs(startCol - endCol) <= 1)
                        return true;
                    break;
            }

            return false;
        }

        private bool IsPathClear(Point start, Point end)
        {
            int rowDiff = Math.Sign(end.X - start.X);
            int colDiff = Math.Sign(end.Y - start.Y);

            int row = start.X + rowDiff;
            int col = start.Y + colDiff;

            while (row != end.X || col != end.Y)
            {
                if (board[row, col] != null)
                {
                    return false;
                }
                row += rowDiff;
                col += colDiff;
            }
            return true;
        }

        private void Checkmate()
        {
            string winner = isWhiteTurn ? "Black" : "White";