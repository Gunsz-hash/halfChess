using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Windows.Forms;
using FinalProject.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace FinalProject
{
    internal class Game
    {

        public delegate void UpdateUIDelegate(Board board, bool isWhiteTurn, int timeLeft, bool isCheck);

        private readonly UpdateUIDelegate updateUI;  // readonly fkor better practice, 
        
        //logic
        internal Board board;
        private bool isWhiteTurn;
        internal Piece SelectedPiece { get; set; }
        private bool clickedFirst;
       
        //timing
        private Timer gameTimer;
        private int timeLeft;
        private int timeLimit = 20; //default

        //game start
        private bool isFirstMove;

        //animations
        private Timer animationTimer;
        private int animationSteps = 0;
        private Point startPos;
        private Point endPos;


        
        //how does a new game get created after one is finished? - keep it for future upgrades


        //ctor

        public Game(UpdateUIDelegate updateUICallback)
        {
            board = new Board();
            board.InitBoard(); //in any way we call it even when its in the ctor, because it made a few problem.         if it works dont touch it ;)
            isWhiteTurn = true;
            SelectedPiece = new EmptyPiece(null);
            clickedFirst = false;
            isFirstMove = true;
            updateUI = updateUICallback;


            InitializeTimer();
           
            //first initialization
            updateUI(board, isWhiteTurn, timeLimit, false);
        }

        

        //game timer

        private void InitializeTimer()
        {
            gameTimer = new Timer
            {
                Interval = 1000// 1 second
            };
            gameTimer.Tick += Timer_Tick;
            timeLeft = timeLimit;
        }
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


        
      
        //clicks and validations (move and then cap)

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



        //checking for moves and moving

        //if not animation needed:



        //public void Move(Square start, Square end)
        //{
        //    Piece OriginalStartPiece = board.GetPiece(start);

        //    board.SetPiece(end, OriginalStartPiece);
        //    board.SetPiece(start, new EmptyPiece(null));
        //}



        //public void Capture(Square start, Square end)
        //{
        //    // Just use SetPiece to handle the capture
        //    Piece movingPiece = board.GetPiece(start);
        //    board.SetPiece(end, movingPiece);  // Place capturing piece
        //    board.SetPiece(start, new EmptyPiece(start));  // Empty original sqr
        //}


        //if animation needed:


        public void Move(Square start, Square end)
        {
            // save original piece and tempor move
            Piece OriginalStartPiece = board.GetPiece(start);
            board.SetPiece(end, OriginalStartPiece);
            board.SetPiece(start, new EmptyPiece(null));

            //buttons for the animation
            Button startButton = ((ChessForm)Form.ActiveForm).boardButtons[start.Row, start.Col];
            Button endButton = ((ChessForm)Form.ActiveForm).boardButtons[end.Row, end.Col];
            startPos = startButton.Location;
            endPos = endButton.Location;

            // animation timer and starting it
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationSteps = 0;
            animationTimer.Tick += (sender, e) => {
                animationSteps++;

                if (animationSteps <= 15) 
                {
                    double progress = (double)animationSteps / 15;
                    int newX = startPos.X + (int)((endPos.X - startPos.X) * progress);
                    int newY = startPos.Y + (int)((endPos.Y - startPos.Y) * progress);

                    startButton.Location = new Point(newX, newY);
                }
                else
                {
                    // animation complete
                    animationTimer.Stop();
                    startButton.Location = startPos; // reset button pos
                    updateUI(board, isWhiteTurn, timeLeft, IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing));
                }
            };

            animationTimer.Start();
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



        public void Capture(Square start, Square end)
        {
            // save original piece and tempor capture
            Piece movingPiece = board.GetPiece(start);
            board.SetPiece(end, movingPiece);  //capturing piece
            board.SetPiece(start, new EmptyPiece(start));  // empty original sqr

            //buttons for the animation
            Button startButton = ((ChessForm)Form.ActiveForm).boardButtons[start.Row, start.Col];
            Button endButton = ((ChessForm)Form.ActiveForm).boardButtons[end.Row, end.Col];
            startPos = startButton.Location;
            endPos = endButton.Location;

            //  animation timer and starting it
            animationTimer = new Timer();
            animationTimer.Interval = 20;
            animationSteps = 0;
            animationTimer.Tick += (sender, e) => {
                animationSteps++;

                if (animationSteps <= 15)
                {
                    double progress = (double)animationSteps / 15;
                    int newX = startPos.X + (int)((endPos.X - startPos.X) * progress);
                    int newY = startPos.Y + (int)((endPos.Y - startPos.Y) * progress);

                    startButton.Location = new Point(newX, newY);
                }
                else
                {
                    // animation complete
                    animationTimer.Stop();
                    startButton.Location = startPos; // reset button pos
                    updateUI(board, isWhiteTurn, timeLeft, IsInCheck(isWhiteTurn ? board.whiteKing : board.blackKing));
                }
            };

            animationTimer.Start();
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


        //avoiding checkmate
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

        //todo : i think we can get much more efficient than 4 nested loops by using another DSA, should check that if we have time
        public bool CanOtherPieceDefend(King king)
        {
            //get the friendly pieces

            for (int row = 0; row < Board.Rows; row++)
            {
                for (int col = 0; col < Board.Columns; col++)
                {
                    Piece defendingPiece = board.GetPiece(new Square(row, col));


                    //if friendly (and not empty/king too)
                    if (!defendingPiece.IsEmpty && defendingPiece.Color == king.Color && defendingPiece.Type != PieceType.King)
                    {

                        //try every possible square on the board
                        for (int targetRow = 0; targetRow < Board.Rows; targetRow++)
                        {
                            for (int targetCol = 0; targetCol < Board.Columns; targetCol++)
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

                                    // check if move prevents check
                                    bool stillInCheck = IsInCheck(king);

                                    // Restore original board 
                                    board.SetPiece(defenderOriginalPos, defendingPiece);
                                    board.SetPiece(targetSquare, targetOriginalPiece);

                                    // If we found defending move
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

        //checking for check/checkmate

        public bool IsInCheck(Piece piece)
        {
            King king = LocateKing(piece);

            return (IsPawnThreat(king) || IsKnightThreat(king) || IsBishopThreat(king) || IsRookThreat(king));
        }
        public bool IsCheckmate()
        {

            //locate current king
            King king = !isWhiteTurn ? board.whiteKing : board.blackKing;




            //check if king is in check
            if (!IsInCheck(king))
            {

                return false;
            }




            //king can avoid checkmate
            if (CanAvoidCheckmate(king))
            {

                return false;
            }



            if (CanOtherPieceDefend(king))
            {

                return false;
            }






            return true;
        }
        public bool IsRookThreat(King king)
        {
            int[] rowDirections = { -1, 1, 0, 0 }; // up and down
            int[] colDirections = { 0, 0, -1, 1 }; // left and right

            for (int i = 0; i < 4; i++)  // loop in the 4 directions
            {
                int row = king.Position.Row;
                int col = king.Position.Col;

                while (board.InBounds(row += rowDirections[i], col += colDirections[i]))
                {
                    Square possibleRook = new Square(row, col);
                    Piece piece = board.GetPiece(possibleRook);

                    // check for oppon rook
                    if (piece.Color != king.Color && (piece.Type == PieceType.Rook))
                    {
                        return true;
                    }

                    // if theres a piece in the way
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

            foreach (var dc in pawnCols) //dc is the possible pawn locations
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

                        // check for opp bishop
                        if (piece.Color != king.Color && piece.Type == PieceType.Bishop)
                        {
                            return true;
                        }

                        //if theres a piece in the way
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



        //overall functions ( for easier coding for me):

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

        public bool ValidTurn(Piece piece)
        {
            return piece.IsWhite == isWhiteTurn;
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



        //game ending and all of its things


        private async void EndGame()
        {
            gameTimer.Stop();
            isFirstMove = true; //reset the first game for the timer in first move (for future upgrades and game restarting)
            updateUI(board, isWhiteTurn, timeLeft, false);


            //num games incrementing
            await IncGames();


            //maybe opening the web - in future upgrades


            Application.Exit();
        }


        private async Task IncGames()
        {
            int id = Convert.ToInt32(Program.player.UserId);
            string path = $"api/TblChessPlayers/incGames/{id}";
            try
            {
                // Add empty content (for PUT)
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");

                var response = await Program.client.PutAsync(path, emptyContent);

                // Read response content 
                string responseContent = await response.Content.ReadAsStringAsync();


                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<GameUpdateResponse>(responseContent);
                    Program.player.NumOfGames = result.currentGames;

                }
                else
                {
                    string message = $"Failed to update games(inc). Status: {response.StatusCode}. Message: {responseContent}";
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }



        //for the inc games

        public class GameUpdateResponse
        {
            public string message { get; set; }
            public int currentGames { get; set; }
        }

    }
}
