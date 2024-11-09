using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Form1 : Form
    {
        private Button[,] boardButtons = new Button[8, 4];
        private string[,] board = new string[8, 4];
        private bool isWhiteTurn = true;  // Track the turn (White starts first)
        private Point whiteKingPosition;
        private Point blackKingPosition;
        private Point? selectedPiece = null;
        private Timer moveTimer;
        //test 3
        private ComboBox intervalComboBox {  get; set; }    

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            PlacePieces();

            // Adjust the form size to make it bigger and provide space for the ComboBox
            this.ClientSize = new Size(5 * 120, 8 * 120 + 100); // Increase form height by 100 for ComboBox area

            // Initialize ComboBox for interval selection and move it away from the board
            intervalComboBox = new ComboBox();  // Initialize the ComboBox
            intervalComboBox.Items.AddRange(new object[] { 20, 30 }); // Add interval choices
            intervalComboBox.SelectedItem = 20; // Default value
            intervalComboBox.Location = new Point(10, 350); // Move ComboBox further down (adjust as needed)
            intervalComboBox.SelectedIndexChanged += IntervalComboBox_SelectedIndexChanged;
            Controls.Add(intervalComboBox);

            // Initialize timer for move delays (or use for other timed events)
            moveTimer = new Timer();
            moveTimer.Interval = 1000; // 1-second interval (for example)
            moveTimer.Tick += MoveTimer_Tick;
        }
        private void IntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedInterval = (int)intervalComboBox.SelectedItem; // No need for "Form1." prefix
            moveTimer.Interval = selectedInterval * 100;// Multiply by 100 to adjust the unit of time
        }
        private void InitializeBoard()
        {
            int tileWidth = 100; // Wider tiles
            int tileHeight = 80; // Adjust height to balance proportions
            Font pieceFont = new Font("Segoe UI Symbol", 24, FontStyle.Bold); // Larger font for pieces

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    var button = new Button();
                    button.Size = new Size(tileWidth, tileHeight);
                    button.Location = new Point(col * tileWidth, row * tileHeight);
                    button.Font = pieceFont;
                    button.Click += Button_Click;
                    button.Tag = new Point(row, col); // Store position in the Tag

                    // Color alternation
                    button.BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;

                    Controls.Add(button);
                    boardButtons[row, col] = button;
                }
            }
        }
        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            // Perform any timed actions here, like updating the game state or piece animations.
            // The moveTimer.Interval is now adjusted based on the ComboBox choice.
        }
        private void PlacePieces()
        {
            board[0, 0] = "♚"; blackKingPosition = new Point(0, 0); // Black King
            board[0, 1] = "♝"; // Black Bishop
            board[0, 2] = "♞"; // Black Knight
            board[0, 3] = "♜"; // Black Rook
            for (int i = 0; i < 4; i++) board[1, i] = "♟"; // Black pawns

            for (int i = 0; i < 4; i++) board[6, i] = "♙"; // White pawns
            board[7, 0] = "♔"; // White King
            whiteKingPosition = new Point(7, 0);
            board[7, 1] = "♗"; // White Bishop
            board[7, 2] = "♘"; // White Knight
            board[7, 3] = "♖"; // White Rook

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

            // Check if the piece at the selected position belongs to the current player
            bool isWhitePiece = board[row, col] != null && "♔♕♖♗♘♙".Contains(board[row, col]);
            bool isBlackPiece = board[row, col] != null && "♚♛♜♝♞♟".Contains(board[row, col]);

            // Prevent selecting an opponent's piece as the initial selection
            if (selectedPiece == null && ((isWhiteTurn && isBlackPiece) || (!isWhiteTurn && isWhitePiece)))
            {
                MessageBox.Show("It is not your turn.");
                return;
            }

            // Handle piece selection
            if (selectedPiece == null)
            {
                // Select the piece only if it belongs to the current player
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

                // If the same piece is clicked again, deselect it
                if (selectedRow == row && selectedCol == col)
                {
                    ResetBoardColors();
                    selectedPiece = null;
                    return;
                }

                // Determine if it's a move to an occupied square of the same side
                bool isDestinationSameSide = (isWhiteTurn && isWhitePiece) || (!isWhiteTurn && isBlackPiece);
                if (isDestinationSameSide)
                {
                    MessageBox.Show("Invalid move. You cannot move to a square occupied by your own piece.");
                    return;
                }

                // Determine if move is a capture or a regular move
                bool isOpponentPiece = (isWhiteTurn && isBlackPiece) || (!isWhiteTurn && isWhitePiece);

                // If it's an opponent's piece, attempt a capture
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
        private bool IsKingCheckmated(bool isWhiteKing)
        {
            // Get the current position of the king
            Point kingPosition = isWhiteKing ? whiteKingPosition : blackKingPosition;
            int kingRow = kingPosition.X;
            int kingCol = kingPosition.Y;

            // List all possible moves for the king (adjacent squares)
            int[] directions = { -1, 0, 1 };
            foreach (int rowDir in directions)
            {
                foreach (int colDir in directions)
                {
                    // Skip the current square (no movement)
                    if (rowDir == 0 && colDir == 0)
                        continue;

                    int newRow = kingRow + rowDir;
                    int newCol = kingCol + colDir;

                    // Check if the new position is within the board limits
                    if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                    {
                        string originalPiece = board[newRow, newCol];

                        // Move the king to this new position temporarily
                        board[kingRow, kingCol] = null;
                        board[newRow, newCol] = isWhiteKing ? "♔" : "♚";

                        // Update the king's temporary position
                        Point tempKingPosition = new Point(newRow, newCol);
                        if (isWhiteKing)
                            whiteKingPosition = tempKingPosition;
                        else
                            blackKingPosition = tempKingPosition;

                        // Check if this move leaves the king in check
                        string checkingPiece = isWhiteKing ? IsKingInCheck(true) : IsKingInCheck(false);
                        bool isSafeMove = checkingPiece == null;

                        // Revert the temporary move
                        board[kingRow, kingCol] = isWhiteKing ? "♔" : "♚";
                        board[newRow, newCol] = originalPiece;
                        if (isWhiteKing)
                            whiteKingPosition = kingPosition;
                        else
                            blackKingPosition = kingPosition;

                        // If there's at least one safe move, king is not in checkmate
                        if (isSafeMove)
                        {
                            return false;
                        }
                    }
                }
            }

            // If no moves can escape check, the king is checkmated
            return true;
        }

        private void CheckForCheckmate()
        {
            // Check if the current player's king is checkmated
            bool isCheckmated = IsKingCheckmated(!isWhiteTurn); // Check the opponent's king
            if (isCheckmated)
            {
                // Stop the game and declare the winner
                string winner = isWhiteTurn ? "White" : "Black";
                MessageBox.Show($"{winner} wins! The opponent's king is checkmated.");
                Application.Exit(); // Or reset the board for a new game, if desired
            }
        }

        private void CanPreformMove(int startRow, int startCol, int endRow, int endCol, bool capture)
        {
            // Capture or move to the new position
            string movingPiece = board[startRow, startCol];
            board[endRow, endCol] = movingPiece;
            board[startRow, startCol] = null;

            // Update king position if moved
            if (movingPiece == "♔") whiteKingPosition = new Point(endRow, endCol);
            if (movingPiece == "♚") blackKingPosition = new Point(endRow, endCol);

            // Reset selection, update turn, and refresh board UI
            selectedPiece = null;
            isWhiteTurn = !isWhiteTurn;
            ResetBoardColors();
            UpdateBoard();

            // Check if the opponent's king is in checkmate after the move
            CheckForCheckmate();
        }

        private void PerformMove(int selectedRow, int selectedCol, int row, int col, bool capture)
        {
            string originalPiece = board[selectedRow, selectedCol];
            string tempEndPiece = board[row, col];

            // Perform the move
            board[row, col] = originalPiece;
            board[selectedRow, selectedCol] = null;

            // Update king position if a king is moved
            if (originalPiece == "♔") whiteKingPosition = new Point(row, col);
            else if (originalPiece == "♚") blackKingPosition = new Point(row, col);

            // Check if move leaves own king in check
            string checkingPiece = isWhiteTurn ? IsKingInCheck(true) : IsKingInCheck(false);
            if (checkingPiece != null)
            {
                // Revert move if it leaves the king in check
                board[selectedRow, selectedCol] = originalPiece;
                board[row, col] = tempEndPiece;

                if (originalPiece == "♔") whiteKingPosition = new Point(selectedRow, selectedCol);
                else if (originalPiece == "♚") blackKingPosition = new Point(selectedRow, selectedCol);

                MessageBox.Show("Invalid move! You cannot leave your king in check.");
            }
            else
            {
                // Move successful, switch turns and notify of capture
                isWhiteTurn = !isWhiteTurn;

                // Display capture message if necessary
                if (capture && tempEndPiece != null)
                {
                    MessageBox.Show($"Captured {tempEndPiece}!");
                }

                // Check if opponent’s king is in check
                string opponentCheckingPiece = IsKingInCheck(!isWhiteTurn);
                if (opponentCheckingPiece != null)
                {
                    MessageBox.Show(isWhiteTurn ? $"Black King is in check by {opponentCheckingPiece}!" : $"White King is in check by {opponentCheckingPiece}!");
                }
               
            }

            // Reset selection and update board
            selectedPiece = null;
            ResetBoardColors();
            UpdateBoard();
        }


        private bool IsPieceWhite(int row, int col)
        {
            // Check if the piece at the given position is white (White pieces: ♔, ♗, ♘, ♖, ♙)
            return board[row, col] != null && "♔♗♘♖♙".Contains(board[row, col]);
        }

        private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
        {
            string piece = board[startRow, startCol];
            bool isWhitePiece = "♔♗♘♖♙".Contains(piece); // White pieces
            bool isBlackPiece = "♚♝♞♜♟".Contains(piece); // Black pieces

            if (piece == null)
            {
                return false; // No piece to move
            }

            if (isWhiteTurn && isBlackPiece || !isWhiteTurn && isWhitePiece)
            {
                return false; // Wrong turn
            }

            switch (piece)
            {
                case "♙": // White Pawn
                    if (startCol == endCol)
                    {
                        // Single square move
                        if (endRow == startRow - 1 && board[endRow, endCol] == null)
                            return true;

                        // First move: two-square move
                        if (startRow == 6 && endRow == startRow - 2 && board[endRow, endCol] == null && board[startRow - 1, startCol] == null)
                            return true;
                    }

                    // Capture
                    if (Math.Abs(startCol - endCol) == 1 && endRow == startRow - 1 && board[endRow, endCol] != null)
                        return true;

                    break;

                case "♟": // Black Pawn
                    if (startCol == endCol)
                    {
                        // Single square move
                        if (endRow == startRow + 1 && board[endRow, endCol] == null)
                            return true;

                        // First move: two-square move
                        if (startRow == 1 && endRow == startRow + 2 && board[endRow, endCol] == null && board[startRow + 1, startCol] == null)
                            return true;
                    }

                    // Capture
                    if (Math.Abs(startCol - endCol) == 1 && endRow == startRow + 1 && board[endRow, endCol] != null)
                        return true;

                    break;

                case "♘": // White Knight
                case "♞": // Black Knight
                    if (Math.Abs(startRow - endRow) == 2 && Math.Abs(startCol - endCol) == 1 ||
                        Math.Abs(startRow - endRow) == 1 && Math.Abs(startCol - endCol) == 2)
                        return true;
                    break;

                case "♗": // White Bishop
                case "♝": // Black Bishop
                    if (Math.Abs(startRow - endRow) == Math.Abs(startCol - endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;

                case "♖": // White Rook
                case "♜": // Black Rook
                    if ((startRow == endRow || startCol == endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;

                case "♕": // White Queen
                case "♛": // Black Queen
                    if ((Math.Abs(startRow - endRow) == Math.Abs(startCol - endCol) || startRow == endRow || startCol == endCol) &&
                        IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;

                case "♔": // White King
                case "♚": // Black King
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

            // Check if path is clear for sliding pieces (Bishop, Rook, Queen)
            while (row != end.X || col != end.Y)
            {
                if (board[row, col] != null)
                {
                    return false; // There is a piece blocking the path
                }
                row += rowDiff;
                col += colDiff;
            }
            return true;
        }


        private string IsKingInCheck(bool checkWhiteKing)
        {
            Point kingPosition = checkWhiteKing ? whiteKingPosition : blackKingPosition;
            string checkingPiece = null;

            // Loop through all board positions to find pieces of the opposite color
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != null && IsPieceColorCorrect(row, col, !checkWhiteKing)) // Check opposite color
                    {
                        string piece = board[row, col];

                        // Check if this piece can attack the king's position
                        if ((piece == "♖" || piece == "♜" || piece == "♕" || piece == "♛") && (row == kingPosition.X || col == kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                return checkingPiece;
                            }
                        }
                        if ((piece == "♗" || piece == "♝" || piece == "♕" || piece == "♛") && Math.Abs(row - kingPosition.X) == Math.Abs(col - kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                return checkingPiece;
                            }
                        }
                        if ((piece == "♘" || piece == "♞") && IsKnightAttack(kingPosition, new Point(row, col)))
                        {
                            checkingPiece = piece;
                            return checkingPiece;
                        }
                    }
                }
            }
            return checkingPiece;
        }

        private bool IsKnightAttack(Point kingPosition, Point piecePosition)
        {
            int rowDiff = Math.Abs(kingPosition.X - piecePosition.X);
            int colDiff = Math.Abs(kingPosition.Y - piecePosition.Y);
            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }


        private bool IsPieceColorCorrect(int row, int col, bool checkWhite)
        {
            if (board[row, col] == null)
                return false;

            return checkWhite ? "♔♗♘♖♙".Contains(board[row, col]) : "♚♝♞♜♟".Contains(board[row, col]);
        }
        private void ResetBoardColors()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    boardButtons[row, col].BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray;
                }
            }
        }

 
        
    }
}
