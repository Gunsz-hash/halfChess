using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    
 
    public partial class Form1 : Form
    {
        /*private int x, y;
        private Bitmap bitmap;
        private Timer flashTimer;
        private bool flashRed = false;*/
        private Button[,] boardButtons = new Button[8, 4];
        //private string[,] board = new string[8, 4];
        //private bool isWhiteTurn = true;  // Track the turn (White starts first)
        private Point whiteKingPosition;
        private Point blackKingPosition;
        private Point? selectedPiece = null;
        private Timer moveTimer;
        //itamar branch

        private ComboBox intervalComboBox { get; set; }




























        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            PlacePieces();

            // Adjust the form size to make it bigger and provide space for the ComboBox
            this.ClientSize = new Size(5 * 120, 8 * 120 + 100); // Increase form height by 100 for ComboBox area

            // Initialize ComboBox for interval selection
            intervalComboBox = new ComboBox();

            // Update interval choices to 20, 60, 90
            intervalComboBox.Items.AddRange(new object[] { 20, 60, 90 });
            intervalComboBox.SelectedItem = 20; // Set default to 20
            intervalComboBox.Location = new Point(10, 350); // Adjust location as needed
            intervalComboBox.SelectedIndexChanged += IntervalComboBox_SelectedIndexChanged;
            Controls.Add(intervalComboBox);

            // Initialize timer for move delays
            moveTimer = new Timer();
            moveTimer.Interval = 2000; // Default interval (20 seconds, set by default ComboBox value)
            moveTimer.Tick += MoveTimer_Tick;
        }

        private Rectangle r = new Rectangle(30, 30, 200, 200);



        private void IntervalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void CheckValidator(bool isWhiteTurn)
        {

        }

        private string IsKingInCheck(bool checkWhiteKing)
        {
            Point kingPosition = checkWhiteKing ? whiteKingPosition : blackKingPosition;
            string checkingPiece = null;


            //

            // Loop through all board positions to find pieces of the opposite color
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    /*if (board[row, col] != null && IsPieceColorCorrect(row, col, !checkWhiteKing)) // Check opposite color
                    {
                        string piece = board[row, col];

                        // Check if this piece can attack the king's position
                        if ((piece == "♖" || piece == "♜") && (row == kingPosition.X || col == kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                //CheckForCheckmate();

                                return checkingPiece;
                            }
                        }
                        if ((piece == "♗" || piece == "♝") && Math.Abs(row - kingPosition.X) == Math.Abs(col - kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                //CheckForCheckmate();

                                return checkingPiece;
                            }
                        }
                        if ((piece == "♘" || piece == "♞") && IsKnightAttack(kingPosition, new Point(row, col)))
                        {
                            checkingPiece = piece;
                            //CheckForCheckmate();
                            return checkingPiece;
                        }*/
                    }
                }
            }
            return checkingPiece;
        }

        private bool IsKingCheckmated(bool isWhiteKing)
        {
            // Check if the king itself has no legal moves
            Point kingPosition = isWhiteKing ? whiteKingPosition : blackKingPosition;
            if (HasSafeKingMoves(kingPosition, isWhiteKing))
            {
                return false; // King has at least one move to avoid check
            }

            // Check if any other piece can block or capture the threatening piece
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != null && IsPieceColorCorrect(row, col, isWhiteKing))
                    {
                        Point startPosition = new Point(row, col);

                        // Try moving this piece to every other position on the board
                        for (int targetRow = 0; targetRow < 8; targetRow++)
                        {
                            for (int targetCol = 0; targetCol < 4; targetCol++)
                            {
                                if (IsValidMove(row, col, targetRow, targetCol))
                                {
                                    // Temporarily make the move
                                    string tempEndPiece = board[targetRow, targetCol];
                                    board[targetRow, targetCol] = board[row, col];
                                    board[row, col] = null;

                                    bool kingInCheckAfterMove = IsKingInCheck(isWhiteKing) != null;

                                    // Revert the move
                                    board[row, col] = board[targetRow, targetCol];
                                    board[targetRow, targetCol] = tempEndPiece;

                                    // If this move stops the king from being in check, it’s not checkmate
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

            // No valid moves to avoid check - king is checkmated
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
                        if ((piece == "♖" || piece == "♜") && (row == kingPosition.X || col == kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                //CheckForCheckmate();

                                return checkingPiece;
                            }
                        }
                        if ((piece == "♗" || piece == "♝") && Math.Abs(row - kingPosition.X) == Math.Abs(col - kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                //CheckForCheckmate();

                                return checkingPiece;
                            }
                        }
                        if ((piece == "♘" || piece == "♞") && IsKnightAttack(kingPosition, new Point(row, col)))
                        {
                            checkingPiece = piece;
                            //CheckForCheckmate();
                            return checkingPiece;
                        }
                    }
                }
            }
            return checkingPiece;
        }


        private void PerformMove(int selectedRow, int selectedCol, int row, int col, bool capture)
        {
            var originalPiece = board[selectedRow, selectedCol];
            var capturedPiece = board[row, col];
            var originalPos = new Point(selectedRow, selectedCol);
            var newPos = new Point(row, col);

            // Make the move
            board[row, col] = originalPiece;
            board[selectedRow, selectedCol] = null;

            // Update king position
            if (originalPiece == "WK") whiteKingPosition = newPos;
            else if (originalPiece == "BK") blackKingPosition = newPos;

            // Validate move doesn't leave own king in check
            if (IsKingInCheck(isWhiteTurn) != null)
            {
                // Revert move
                board[selectedRow, selectedCol] = originalPiece;
                board[row, col] = capturedPiece;

                if (originalPiece == "WK") whiteKingPosition = originalPos;
                else if (originalPiece == "BK") blackKingPosition = originalPos;

                MessageBox.Show("Invalid move! You cannot leave your king in check.");
                return;
            }

            // Move is valid, switch turns
            isWhiteTurn = !isWhiteTurn;

            // Handle capture notification
            if (capture && capturedPiece != null)
            {
                MessageBox.Show($"Captured {capturedPiece}!");
            }

            // Check if opponent is now in check
            var checkingPiece = CheckValidator(!isWhiteTurn);
            if (checkingPiece != null)
            {
                var kingColor = isWhiteTurn ? "Black" : "White";
                MessageBox.Show($"{kingColor} King is in check by {checkingPiece}!");
            }

            // Cleanup
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

                        // Temporarily move king
                        board[kingPosition.X, kingPosition.Y] = null;
                        board[newRow, newCol] = isWhiteKing ? "♔" : "♚";

                        bool isSafe = IsKingInCheck(isWhiteKing) == null;

                        // Revert king position
                        board[kingPosition.X, kingPosition.Y] = isWhiteKing ? "♔" : "♚";
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

        //private bool IsPieceWhite(int row, int col)
        //{
        //    // Check if the piece at the given position is white (White pieces: ♔, ♗, ♘, ♖, ♙)
        //    return board[row, col] != null && "♔♗♘♖♙".Contains(board[row, col]);
        //}

        //private bool CheckForCheckmate()
        //{
        //    if (


        //}


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
                case "WP": // White Pawn
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

                case "BP": // Black Pawn
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

                case "WN": // White Knight
                case "BN": // Black Knight
                    if (Math.Abs(startRow - endRow) == 2 && Math.Abs(startCol - endCol) == 1 ||
                        Math.Abs(startRow - endRow) == 1 && Math.Abs(startCol - endCol) == 2)
                        return true;
                    break;

                case "WB": // White Bishop
                case "BB": // Black Bishop
                    if (Math.Abs(startRow - endRow) == Math.Abs(startCol - endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;

                case "WR": // White Rook
                case "BR": // Black Rook
                    if ((startRow == endRow || startCol == endCol) && IsPathClear(new Point(startRow, startCol), new Point(endRow, endCol)))
                        return true;
                    break;



                case "WK": // White King
                case "BK": // Black King
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

        private void Checkmate()
        {

            // Stop the game and declare the winner
            string winner = isWhiteTurn ? "Black" : "White";
            MessageBox.Show($"{winner} wins! The opponent's king is checkmated.");
            Application.Exit(); // Or reset the board for a new game, if desired

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
                        if ((piece == "♖" || piece == "♜") && (row == kingPosition.X || col == kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                //CheckForCheckmate();

                                return checkingPiece;
                            }
                        }
                        if ((piece == "♗" || piece == "♝") && Math.Abs(row - kingPosition.X) == Math.Abs(col - kingPosition.Y))
                        {
                            if (IsPathClear(kingPosition, new Point(row, col)))
                            {
                                checkingPiece = piece;
                                //CheckForCheckmate();

                                return checkingPiece;
                            }
                        }
                        if ((piece == "♘" || piece == "♞") && IsKnightAttack(kingPosition, new Point(row, col)))
                        {
                            checkingPiece = piece;
                            //CheckForCheckmate();
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

        /*private void Form1_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered = true;
            bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);

        }*/


       /* private void Form1_Paint(object sender, PaintEventArgs e)
        {
            const int SIZE = 5;
            Graphics g = Graphics.FromImage(bitmap);
            g.FillEllipse(Brushes.AliceBlue, x, y, SIZE, SIZE);
            e.Graphics.DrawImage(bitmap, x, y);

            g.Dispose();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
                this.Invalidate();  // Trigger a repaint
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X;
                y = e.Y;
                this.Invalidate();
                //this.Update();
            }



        }*/
    }


    

    public class ChessPiece
    {
        public PieceType Type { get; set; }
        public PieceColor Color { get; set; }
        public Position Position { get; set; }
        public bool HasMoved { get; set; }

        public ChessPiece(PieceType type, PieceColor color, Position position)
        {
            Type = type;
            Color = color;
            Position = position;
            HasMoved = false;
        }
    }

    public class ChessBoard
    {
        private ChessPiece[,] board;
        public PieceColor CurrentTurn { get; private set; }
        public bool IsCheck { get; private set; }

        public ChessBoard()
        {
            board = new ChessPiece[8, 4];
            InitializeBoard();
            CurrentTurn = PieceColor.White;
            IsCheck = false;
        }

        private void InitializeBoard()
        {
            // Initialize empty board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    board[row, col] = new ChessPiece(PieceType.Empty, PieceColor.None, new Position(row, col));
                }
            }

            // Set up white pieces
            SetupPieces(PieceColor.White, 7);
            SetupPawns(PieceColor.White, 6);

            // Set up black pieces
            SetupPieces(PieceColor.Black, 0);
            SetupPawns(PieceColor.Black, 1);
        }

        private void SetupPieces(PieceColor color, int row)
        {
            board[row, 0] = new ChessPiece(PieceType.Rook, color, new Position(row, 0));
            board[row, 1] = new ChessPiece(PieceType.Knight, color, new Position(row, 1));
            board[row, 2] = new ChessPiece(PieceType.Bishop, color, new Position(row, 2));
            board[row, 3] = new ChessPiece(PieceType.King, color, new Position(row, 3));
        }

        private void SetupPawns(PieceColor color, int row)
        {
            for (int col = 0; col < 4; col++)
            {
                board[row, col] = new ChessPiece(PieceType.Pawn, color, new Position(row, col));
            }
        }

        public bool MovePiece(Position from, Position to)
        {
            if (!from.IsValid() || !to.IsValid())
                return false;

            ChessPiece piece = board[from.Row, from.Col];

            if (piece.Color != CurrentTurn)
                return false;

            if (!IsValidMove(from, to))
                return false;

            // Store the original state for check validation
            ChessPiece capturedPiece = board[to.Row, to.Col];
            board[to.Row, to.Col] = piece;
            board[from.Row, from.Col] = new ChessPiece(PieceType.Empty, PieceColor.None, from);
            piece.Position = to;
            piece.HasMoved = true;

            // Check if the move puts or leaves the current player in check
            if (IsInCheck(CurrentTurn))
            {
                // Revert the move
                board[from.Row, from.Col] = piece;
                board[to.Row, to.Col] = capturedPiece;
                piece.Position = from;
                return false;
            }

            // Update check status for opponent
            IsCheck = IsInCheck(CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White);

            // Switch turns
            CurrentTurn = CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;

            return true;
        }

        public bool IsValidMove(Position from, Position to)
        {
            ChessPiece piece = board[from.Row, from.Col];

            // Check if destination has friendly piece
            if (board[to.Row, to.Col].Color == piece.Color)
                return false;

            // Special case for horizontal pawn movement (new rule)
            if (piece.Type == PieceType.Pawn && from.Row == to.Row &&
                Math.Abs(from.Col - to.Col) == 1 && board[to.Row, to.Col].Type == PieceType.Empty)
            {
                return true;
            }

            switch (piece.Type)
            {
                case PieceType.King:
                    return IsValidKingMove(from, to);
                case PieceType.Rook:
                    return IsValidRookMove(from, to);
                case PieceType.Bishop:
                    return IsValidBishopMove(from, to);
                case PieceType.Knight:
                    return IsValidKnightMove(from, to);
                case PieceType.Pawn:
                    return IsValidPawnMove(from, to);
                default:
                    return false;
            }
        }

        private bool IsValidKingMove(Position from, Position to)
        {
            int rowDiff = Math.Abs(to.Row - from.Row);
            int colDiff = Math.Abs(to.Col - from.Col);
            return rowDiff <= 1 && colDiff <= 1;
        }

        private bool IsValidRookMove(Position from, Position to)
        {
            if (from.Row != to.Row && from.Col != to.Col)
                return false;

            return !IsPathBlocked(from, to);
        }

        private bool IsValidBishopMove(Position from, Position to)
        {
            if (Math.Abs(to.Row - from.Row) != Math.Abs(to.Col - from.Col))
                return false;

            return !IsPathBlocked(from, to);
        }

        private bool IsValidKnightMove(Position from, Position to)
        {
            int rowDiff = Math.Abs(to.Row - from.Row);
            int colDiff = Math.Abs(to.Col - from.Col);
            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }

        private bool IsValidPawnMove(Position from, Position to)
        {
            int direction = (board[from.Row, from.Col].Color == PieceColor.White) ? -1 : 1;
            int rowDiff = to.Row - from.Row;
            int colDiff = Math.Abs(to.Col - from.Col);

            // Regular move
            if (colDiff == 0 && rowDiff == direction && board[to.Row, to.Col].Type == PieceType.Empty)
                return true;

            // First move - can move two squares
            if (!board[from.Row, from.Col].HasMoved && colDiff == 0 && rowDiff == 2 * direction &&
                board[to.Row, to.Col].Type == PieceType.Empty &&
                board[from.Row + direction, from.Col].Type == PieceType.Empty)
                return true;

            // Capture
            if (colDiff == 1 && rowDiff == direction && board[to.Row, to.Col].Color != board[from.Row, from.Col].Color &&
                board[to.Row, to.Col].Type != PieceType.Empty)
                return true;

            return false;
        }

        private bool IsPathBlocked(Position from, Position to)
        {
            int rowStep = Math.Sign(to.Row - from.Row);
            int colStep = Math.Sign(to.Col - from.Col);

            int currentRow = from.Row + rowStep;
            int currentCol = from.Col + colStep;

            while (currentRow != to.Row || currentCol != to.Col)
            {
                if (board[currentRow, currentCol].Type != PieceType.Empty)
                    return true;

                currentRow += rowStep;
                currentCol += colStep;
            }

            return false;
        }

        public bool IsInCheck(PieceColor color)
        {
            // Find king position
            Position kingPos = FindKing(color);

            // Check if any opponent piece can capture the king
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col].Color != color && board[row, col].Type != PieceType.Empty)
                    {
                        if (IsValidMove(new Position(row, col), kingPos))
                            return true;
                    }
                }
            }

            return false;
        }

        private Position FindKing(PieceColor color) //todo: use another search
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col].Type == PieceType.King && board[row, col].Color == color)
                        return new Position(row, col);
                }
            }
            throw new Exception("King not found - invalid board state");
        }

        public bool IsCheckmate()
        {
            if (!IsCheck)
                return false;

            // Try all possible moves for current player
            for (int fromRow = 0; fromRow < 8; fromRow++)
            {
                for (int fromCol = 0; fromCol < 4; fromCol++)
                {
                    if (board[fromRow, fromCol].Color == CurrentTurn)
                    {
                        for (int toRow = 0; toRow < 8; toRow++)
                        {
                            for (int toCol = 0; toCol < 4; toCol++)
                            {
                                Position from = new Position(fromRow, fromCol);
                                Position to = new Position(toRow, toCol);

                                if (IsValidMove(from, to))
                                {
                                    // Try the move
                                    ChessPiece capturedPiece = board[to.Row, to.Col];
                                    ChessPiece movingPiece = board[from.Row, from.Col];

                                    board[to.Row, to.Col] = movingPiece;
                                    board[from.Row, from.Col] = new ChessPiece(PieceType.Empty, PieceColor.None, from);
                                    movingPiece.Position = to;

                                    bool stillInCheck = IsInCheck(CurrentTurn);

                                    // Undo the move
                                    board[from.Row, from.Col] = movingPiece;
                                    board[to.Row, to.Col] = capturedPiece;
                                    movingPiece.Position = from;

                                    if (!stillInCheck)
                                        return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public ChessPiece GetPiece(Position pos)
        {
            if (!pos.IsValid())
                throw new ArgumentException("Invalid position");
            return board[pos.Row, pos.Col];
        }

        public List<Position> GetValidMoves(Position from)
        {
            List<Position> validMoves = new List<Position>();

            if (!from.IsValid() || board[from.Row, from.Col].Color != CurrentTurn)
                return validMoves;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Position to = new Position(row, col);
                    if (IsValidMove(from, to))
                    {
                        // Test if move would put or leave player in check
                        ChessPiece capturedPiece = board[to.Row, to.Col];
                        ChessPiece movingPiece = board[from.Row, from.Col];

                        board[to.Row, to.Col] = movingPiece;
                        board[from.Row, from.Col] = new ChessPiece(PieceType.Empty, PieceColor.None, from);
                        movingPiece.Position = to;

                        if (!IsInCheck(CurrentTurn))
                            validMoves.Add(to);

                        // Undo the move
                        board[from.Row, from.Col] = movingPiece;
                        board[to.Row, to.Col] = capturedPiece;
                        movingPiece.Position = from;
                    }
                }
            }

            return validMoves;
        }
    }

}