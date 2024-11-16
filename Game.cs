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

        internal Board board;
        private bool isWhiteTurn;
        internal Piece selectedPiece { get; set; }
        private bool clickedFirst;
        private Button[,] boardButtons;

        public Game(Button[,] buttons)
        {
            board = new Board();
            isWhiteTurn = true;
            selectedPiece = new EmptyPiece(null);
            clickedFirst = false;
            boardButtons = buttons; // init the ui buttons
        }



        public bool HandleSquareClick(Square position, Button clickedButton)
        {
            if (selectedPiece.IsEmpty || clickedFirst == false)//first time clicking on a piece
            {
                if (!board.GetPiece(position).IsEmpty) // if there is a piece
                {
                    selectedPiece = board.GetPiece(position); // select that piece
                    clickedFirst = true; // approve clicked once

                    if (Check1stPressValidity(selectedPiece.Position, clickedButton)) //if this function is true, click 1 was made
                    {

                        // what happenes here?
                    }
                }
                
            }
            else
            {
                if(Check2ndPressValidity(selectedPiece.Position, position))//if this function was true, click 2 was made
                {
                    clickedFirst = false;
                }
                else
                {
                    //ignored somehow
                    
                }
                
            }

        }

        public bool Check1stPressValidity(Square position,Button clickedButton)
        {
            Piece piece = board.GetPiece(position);

            if (board.InBounds(position))
            {
                if (ValidTurn(piece)) //turn and piece same color
                {
                    clickedButton.BackColor = Color.Yellow;// highlight player   
                    return true;

                }
                else // turn and piece opposite colors
                {
                    MessageBox.Show("Not Your Turn");
                    selectedPiece = new EmptyPiece(null);
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
                    if (mainPiece.IsValidMove(start, end, board)) //if this square can be accessed 
                    {
                        if (IsInCheck(mainPiece))
                        {
                            if (CanAvoidCheck(start, end))
                            {
                                Move(start, end);
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
                            Move(start, end);
                            return true;
                        }
                    }
                    else //do nothing, print a message, 
                    {
                        //ignore
                    }
                    //logic for testing and then moving
                }


                //if this function is the 2nd press, 
                else if (targetSquare.Color == mainPiece.Color) // if its a friendly piece
                {
                    //ignore
                    //change context of the first click.
                    //exit this function and change the clicked soldier to be it.
                }


                else// enemy
                {
                    if (mainPiece.IsValidMove(start, end, board)) //if this square can be accessed 
                    {
                        //check if im in check and if i am, check if the next move blocks the check, if not - cant do
                        //capture
                    }
                    else //do nothing, print a message, 
                    {
                        //ignore
                    }
                }
            }
            else // not in bounds
            {
                return false;
            }

            
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
        

        public void Move(Square start, Square end)
        {
            Piece OriginalStartPiece = board.GetPiece(start);

            board.SetPiece(end, OriginalStartPiece);
            board.SetPiece(start, new EmptyPiece(null));
        }

        public bool IsInCheck(Piece piece)
        {
            King king = LocateKing(piece);

            //logic if in check return true;


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
        }


        /*public bool MakeMove(Square start, Square end)  //change the null into empty 
        {
            Piece piece = board.GetPiece(start);

            if (piece != null && ValidTurn(piece))
            {
                if (board.MovePiece(start, end))
                {
                    SwitchTurn();
                    return true;
                }
            }
            // Print or log invalid move feedback
            return false;
        }*/


        public bool ValidTurn(Piece piece)
        {
            return piece.IsWhite == isWhiteTurn;
        }

        //valid move





        /*

            if clickedPiece turn:

                clickedPiece is marked, and flag raised

                if(click is in reach & valid - for the currect piece):
                    if nextClick = clickedPiece:
                            cancel mark and, flag
                    else if nextClick = friendlyPiece
                            friendlyPiece marked instead
                    else if nextClick = hostilePiece
                            kill other piece
                    else (empty place)
                            move to the place

                else(not in reach):
                    if nextClick = friendlyPiece
                            friendlyPiece marked instead

               else:
                print "not your turn"



         */


        //is check
        //check var
        //can king escape?
        //is checkmate

        //capture
        //can capture (if not in check)

        //isgameover(reset for a new board)

        //switch turn
        //get turn


        //make move

        //can move?




    }
}
