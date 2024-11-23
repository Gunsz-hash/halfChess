using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    internal class Game
    {

        public delegate void UpdateUIDelegate(Board board, bool isWhiteTurn, int timeLeft, bool isCheck);
        private readonly UpdateUIDelegate updateUI;  // Use our defined delegate type

        internal Board board;
        private bool isWhiteTurn;
        internal Piece SelectedPiece { get; set; }
        private bool clickedFirst;
       // not sure why not here :private Button[,] boardButtons;

        //@
        private Timer gameTimer;
        private int timeLeft;
        private int timeLimit = 20; //default


        private bool isFirstMove;


        //@


        //how does a new game get created after one is finished?

        public Game(UpdateUIDelegate updateUICallback)
        {
            board = new Board();
            board.InitBoard();
            isWhiteTurn = true;
            SelectedPiece = new EmptyPiece(null);
            clickedFirst = false;
            isFirstMove = true;
            updateUI = updateUICallback;


            InitializeTimer();
            //@
            updateUI(board, isWhiteTurn, timeLimit, false);
        }

        //@
        private void InitializeTimer()
        {
            gameTimer = new Timer
            {
                Interval = 1000// 1 second
            };
            gameTimer.Tick += Timer_Tick;
            timeLeft = timeLimit;
        }
        //@

        public void SetTimeLimit(int seconds)
        {
            bool wasRunning = gameTimer.Enabled;
            if (wasRunning)
            {
                gameTimer.Stop();

            }

            timeLimit = seconds;
            timeLeft = seconds;
            updateUI(board, isWhiteTurn, timeLeft, IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing));

            if (wasRunning)
            {
                gameTimer.Start();
            }

        }


        //@
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;

            updateUI(board, isWhiteTurn, timeLeft, IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing));


            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                string winner = isWhiteTurn ? "Black" : "White";
                MessageBox.Show($"Time's up! {winner} wins!");//why?
                EndGame();
            }
        }
        //@


        public void HandleSquareClick(Square position, ChessForm form)
        {
            if (SelectedPiece.IsEmpty || !clickedFirst)//first time clicking on a piece
            {
                if (!board.GetPiece(position).IsEmpty) // if there is a piece
                {
                    SelectedPiece = board.GetPiece(position); // select that piece
                    clickedFirst = true; // approve clicked once

                    if (Check1stPressValidity(SelectedPiece.Position))
                    {
                        form.HighlightSquare(SelectedPiece.Position, Color.Yellow);
                        //optionally, maybe add later:
                        // form.ShowValidMoves(GetValidMoves(position));


                        if (isFirstMove)//the first player move starts the game
                        {
                            gameTimer.Start();
                            isFirstMove = false;
                            form.DisableTimeSelection();
                        }
                    }
                }
                
            }
            
            else
            {
                if(Check2ndPressValidity(SelectedPiece.Position, position))//if this function was true, click 2 was made
                {
                    form.ResetBoardColors();

                    // the case which the move was successful
                    gameTimer.Stop();
                    //form.ResetBoardColors();

                 
                    //check if the game end
                    if (IsCheckmate())
                    {
                        string winner = !isWhiteTurn ? "Black" : "White";
                        MessageBox.Show($"Checkmate! {winner} wins!");
                        EndGame();
                        //reset game?

                    }


                    else
                    {
                        bool isInCheck = IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing);
                        if (isInCheck)
                        {
                            MessageBox.Show("Check!");
                        }
                        updateUI(board, isWhiteTurn, timeLeft, isInCheck);
                        SwitchTurn();

                    }
                }
                else
                {
                    //should really do it? check!
                    form.ResetBoardColors();
                    SelectedPiece = new EmptyPiece(null);
                    clickedFirst = false;

                    // Check if we're in check after an invalid move
                    bool currentlyInCheck = IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing);
                    updateUI(board, isWhiteTurn, timeLeft, currentlyInCheck);

                }
               
                
            }

            

        }

        public bool Check1stPressValidity(Square position)
        {
            Piece piece = board.GetPiece(position);

            if (board.InBounds(position))
            {
                if (ValidTurn(piece)) //turn and piece same color
                {   
                    return true;

                }
                else // turn and piece opposite colors
                {
                    MessageBox.Show("Not Your Turn");
                    SelectedPiece = new EmptyPiece(null);
                    clickedFirst = false;
                    return false; //not your turn
                }
            }
            else//not in bounds
            {
                //ignore
                return false;
            }
            

        }

        private void EndGame()
        {
            gameTimer.Stop();
            isFirstMove = true; //reset the first turn for the timer in first move
            updateUI(board, isWhiteTurn, timeLeft, false);


            //todo logic for opening the web


            Application.Exit();
        }

        public bool Check2ndPressValidity(Square start, Square end)
        {
            Piece mainPiece = board.GetPiece(start);
            Piece targetSquare = board.GetPiece(end);
            if (board.InBounds(end))
            {
                if (targetSquare.IsEmpty)
                {
                    if (mainPiece.IsValidMove(start, end, board))
                    {
                        if (IsInCheck(mainPiece)) //if already in check
                        {
                            if (CanAvoidCheck(start, end))
                            {
                                Move(start, end);
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Invalid move! You cannot leave your king in check.");
                                updateUI(board, isWhiteTurn, timeLeft, true);
                                return false;
                            }
                        }
                        else
                        {
                            Move(start, end);
                            if (IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing))
                            {
                                Move(end, start);
                                MessageBox.Show("Invalid move! This would put you in check.");
                                updateUI(board, isWhiteTurn, timeLeft, true);
                                return false;
                            }
                            return true;
                        }
                    }
                }
                else if (targetSquare.Color == mainPiece.Color) // if its a friendly piece
                {
                    SelectedPiece = board.GetPiece(end);
                    return false;
                }
                else // enemy
                {
                    if (mainPiece.IsValidMove(start, end, board))
                    {
                        if (IsInCheck(mainPiece))
                        {
                            if (CanAvoidCheckByCapture(start, end))
                            {
                                Capture(start, end);
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Invalid move! You cannot leave your king in check.");
                                return false;
                            }
                        }
                        else
                        {
                            // Test if the capture would put us in check
                            Piece movingPiece = board.GetPiece(start);
                            Piece targetPiece = board.GetPiece(end);

                            // make the move tempor
                            board.SetPiece(end, movingPiece);
                            board.SetPiece(start, new EmptyPiece(start));

                            // if this capture puts our king in check
                            if (IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing))
                            {
                                // Undo the temporary move
                                board.SetPiece(start, movingPiece);
                                board.SetPiece(end, targetPiece);
                                MessageBox.Show("Invalid capture! This would put you in check.");
                                updateUI(board, isWhiteTurn, timeLeft, true);
                                return false;
                            }

                            // Undotemp move
                            board.SetPiece(start, movingPiece);
                            board.SetPiece(end, targetPiece);

                            // If we got here, the capture is safe, so  capture
                            Capture(start, end);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CanAvoidCheck(Square start, Square end)
        {
            Piece OriginalStartPiece = board.GetPiece(start);
            Piece OriginalEndPiece = board.GetPiece(end);


            //DO THE MOVE
            board.SetPiece(end, OriginalStartPiece);
            board.SetPiece(start, new EmptyPiece(null));

            //if avoided
            bool avoided = !IsInCheck(OriginalStartPiece);

            //undo move
            board.SetPiece(start, OriginalStartPiece);
            board.SetPiece(end, OriginalEndPiece);

            return avoided;
        }

        public bool CanAvoidCheckmate(King king)
        {
            int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < Board.Rows; i++)
            {
                int newRow = king.Position.Row + rowOffsets[i];
                int newCol = king.Position.Col + colOffsets[i];
                Square newPosition = new Square(newRow, newCol);


                if (board.InBounds(newPosition))
                {
                    Piece target = board.GetPiece(newPosition);

                    if (target.Color != king.Color)//if not friedndly
                    {
                        if (CanAvoidCheck(king.Position, newPosition) || CanAvoidCheckByCapture(king.Position, newPosition))
                        {
                            return true;
                        }

                    }


                }

            }

            return false; // No legal moves for the king
        }


        public void Capture(Square start, Square end)
        {
            // Just use SetPiece to handle the capture
            Piece movingPiece = board.GetPiece(start);
            board.SetPiece(end, movingPiece);  // Place capturing piece
            board.SetPiece(start, new EmptyPiece(start));  // Empty original sqr
        }

        public bool CanAvoidCheckByCapture(Square start, Square end)
        {
            Piece capturedOriginal = board.GetPiece(end);
            Piece captured = board.GetPiece(end);

            Piece OriginalStartPiece = board.GetPiece(start);
            Piece OriginalEndPiece = board.GetPiece(end);


            //DO THE MOVE
            board.SetPiece(end, OriginalStartPiece);
            board.SetPiece(start, new EmptyPiece(null)); //isnt that the function move?
            captured.Color = PieceColor.None;
            captured.SetPosition(null);
            captured.Type = PieceType.Empty;

            //if avoided
            bool avoided = !IsInCheck(OriginalStartPiece);

            //undo move
            board.SetPiece(start, OriginalStartPiece);
            board.SetPiece(end, OriginalEndPiece);

            captured.Color = capturedOriginal.Color;
            captured.SetPosition(capturedOriginal.Position);
            captured.Type = capturedOriginal.Type;

            return avoided;
        }

        public void Move(Square start, Square end)
        {
            Piece OriginalStartPiece = board.GetPiece(start);

            board.SetPiece(end, OriginalStartPiece);
            board.SetPiece(start, new EmptyPiece(null));
        }


        public bool IsRookThreat(King king)
        {
            int[] rowDirections = { -1, 1, 0, 0 }; // Up, Down
            int[] colDirections = { 0, 0, -1, 1 }; // Left, Right

            for (int i = 0; i < 4; i++)  // Loop through each straight-line direction
            {
                int row = king.Position.Row;
                int col = king.Position.Col;

                while (board.InBounds(row += rowDirections[i], col += colDirections[i]))
                {
                    Square possibleRook = new Square(row, col);
                    Piece piece = board.GetPiece(possibleRook);

                    // Check if it's an opponent's rook
                    if (piece.Color != king.Color && (piece.Type == PieceType.Rook))
                    {
                        return true;
                    }

                    // Stop if there is any other piece in the way
                    if (!piece.IsEmpty)
                    {
                        break;
                    }
                }
            }

            return false;
        }
        public bool IsPawnThreat(King king)
        {
            int pawnDirection = king.IsWhite ? -1 : 1;// white up, black down;
            int[] pawnCols = { -1, 1 }; // pawn attack diagonally

            foreach(var dc in pawnCols) //dc is the possible pawn locations
            {
                int row = king.Position.Row + pawnDirection;
                int col = king.Position.Col + dc;
                Square possiblePawn = new Square(row, col);
                if (board.InBounds(possiblePawn))
                {


                    if (king.IsWhite)
                    {
                        if ((board.GetPiece(possiblePawn).Color == PieceColor.Black) && (board.GetPiece(possiblePawn).Type == PieceType.Pawn))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if ((board.GetPiece(possiblePawn).Color == PieceColor.White) && (board.GetPiece(possiblePawn).Type == PieceType.Pawn))
                        {
                            return true;
                        }

                    }

                }
                else // not a possible threat
                {

                }
            }
            return false;
        }
        public bool IsBishopThreat(King king)
        {
            int[] directions = { -1, 1 };

            foreach (var dr in directions)
            {
                foreach (var dc in directions)
                {
                    int row = king.Position.Row;
                    int col = king.Position.Col;

                    while (board.InBounds(row += dr, col += dc))
                    {
                        Square possibleBishop = new Square(row, col);
                        Piece piece = board.GetPiece(possibleBishop);

                        // Check if it's an opponent's bishop
                        if (piece.Color != king.Color && piece.Type == PieceType.Bishop)
                        {
                            return true;
                        }

                        // Stop if there is any other piece in the way
                        if (!piece.IsEmpty)
                        {
                            break;
                        }
                    }
                }
            }

            return false;
        }
        public bool IsKnightThreat(King king)
        {
            int[] knightMoves = { -2, -1, 1, 2 };

            foreach (var dr in knightMoves)
            {
                foreach (var dc in knightMoves)
                {
                    if (Math.Abs(dr) != Math.Abs(dc))
                    {
                        int row = king.Position.Row + dr;
                        int col = king.Position.Col + dc;
                        Square possibleKnight = new Square(row, col);

                        if (board.InBounds(possibleKnight))
                        {
                            if (king.IsWhite)
                            {

                                if ((board.GetPiece(possibleKnight).Color == PieceColor.Black) && (board.GetPiece(possibleKnight).Type == PieceType.Knight))
                                {
                                    return true;
                                }

                            }
                            else
                            {
                                if ((board.GetPiece(possibleKnight).Color == PieceColor.White) && (board.GetPiece(possibleKnight).Type == PieceType.Knight))
                                {
                                    return true;
                                }

                            }

                        }
                        else//not a possible threat
                        {

                        }

                    }
                }
            }
            return false;

        }

        

        public bool IsInCheck(Piece piece)
        {
            King king = LocateKing(piece);


            //logic if in check return true;

            //didnt add the is valid position

            //checking for each attack:

            return (IsPawnThreat(king) || IsKnightThreat(king) || IsBishopThreat(king) || IsRookThreat(king));
        }

        public bool IsCheckmate()
        {
            //locate current king
            King king = !isWhiteTurn ? board.whiteKing : board.blackKing;


            Console.WriteLine($"Checking checkmate for {(isWhiteTurn ? "White" : "Black")} king");

            //check if king is in check
            if (!IsInCheck(king))
            {
                Console.WriteLine("Not in Check");
                return false;
            }
            Console.WriteLine("King is in check");



            //king can avoid checkmate
            if (CanAvoidCheckmate(king))
            {
                Console.WriteLine("King can move to safety");
                return false;
            }
            Console.WriteLine("King cannot avoid check");


            if (CanOtherPieceDefend(king))
            {
                Console.WriteLine("Another Piece can defend the king");
                return false;
            }
            Console.WriteLine("No piece can defend, its checkmate!");

            
           


            return true;
        }

        //todo : i think we can get much more efficient than 4 nested loops by using another DSA, should check that if we have time
        public bool CanOtherPieceDefend(King king)
        {
            //get the friendly pieces
            
            for(int row = 0; row < Board.Rows; row++)
            {
                for(int col = 0; col < Board.Columns; col++)
                {
                    Piece defendingPiece = board.GetPiece(new Square(row, col));


                    //if friendly (and not empty/king too)
                    if(!defendingPiece.IsEmpty && defendingPiece.Color == king.Color && defendingPiece.Type != PieceType.King)
                    {

                        //try every possible square on the board
                        for(int targetRow = 0; targetRow < Board.Rows; targetRow++)
                        {
                            for(int targetCol = 0; targetCol < Board.Columns; targetCol++)
                            {
                                Square targetSquare = new Square(targetRow, targetCol);
                                Piece targetPiece = board.GetPiece(targetSquare);


                                //if the move it valid
                                if (defendingPiece.IsValidMove(defendingPiece.Position, targetSquare, board))
                                {
                                    // Store original positions and pieces
                                    Square defenderOriginalPos = defendingPiece.Position;
                                    Piece targetOriginalPiece = targetPiece;

                                    // Make the move/capture
                                    board.SetPiece(targetSquare, defendingPiece);
                                    board.SetPiece(defenderOriginalPos, new EmptyPiece(defenderOriginalPos));

                                    // Check if this move prevents check
                                    bool stillInCheck = IsInCheck(king);

                                    // Restore the original board state
                                    board.SetPiece(defenderOriginalPos, defendingPiece);
                                    board.SetPiece(targetSquare, targetOriginalPiece);

                                    // If we found a defending move
                                    if (!stillInCheck)
                                    {
                                        Console.WriteLine($"Defense found: {defendingPiece.Type} can " +
                                            (targetOriginalPiece.IsEmpty ? "move to" : "capture at") +
                                            $" {targetRow},{targetCol}");
                                        return true;
                                    }



                                }



                            }
                        }

                    }


                }
            }
            Console.WriteLine("No defensive moves or captures found");
            return false;
        }




        public King LocateKing(Piece piece)
        {
            if (piece.IsWhite)
            {
                return board.whiteKing;
            }
            else
            {
                return board.blackKing;
            }
        }


        private void SwitchTurn()
        {
            isWhiteTurn = !isWhiteTurn;
            clickedFirst = false;
            SelectedPiece = new EmptyPiece(null);
            timeLeft = timeLimit;
            gameTimer.Start();
            updateUI(board, isWhiteTurn, timeLeft, IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing));
        }


        


        public bool ValidTurn(Piece piece)
        {
            return piece.IsWhite == isWhiteTurn;
        }

        //valid move





       



    }
}
