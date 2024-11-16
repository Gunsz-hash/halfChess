using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Game
    {

        internal Board board;
        private bool isWhiteTurn;
        internal Piece selectedPiece { get; set; }
        private bool clickedFirst;

        public Game()
        {
            board = new Board();
            isWhiteTurn = true;
            selectedPiece = new EmptyPiece(null);
            clickedFirst = false;
        }



        public bool SquareClick(Square position)
        {
            if (selectedPiece.IsEmpty || clickedFirst == false)//first time clicking on a piece
            {
                if (!board.GetPiece(position).IsEmpty) // if 
                {
                    selectedPiece = board.GetPiece(position);
                    clickedFirst = true;

                    if (Check1stPressValidty(selectedPiece.Position)) //if this function is true, click 1 was made
                    {

                    }
                    else
                    {
                        selectedPiece = new EmptyPiece(null);
                        clickedFirst = false;
                    }
                }
                
            }
            else
            {
                if(Check2ndPressValidty(selectedPiece.Position, position))//if this function was true, click 2 was made
                {
                    clickedFirst = false;
                }
                else
                {
                    //ignored somehow
                    
                }
                
            }

        }

        public bool Check1stPressValidty(Square position)
        {
            Piece piece = board.GetPiece(position);


            if (board.InBounds(position.Row, position.Col))
            {
                if (ValidTurn(piece)) //turn and piece same color
                {
                    //mark the piece




                }
                else // turn and piece opposite colors
                {
                    //push a message
                }
            }
            else
            {

            }

        }

        public bool Check2ndPressValidty(Square start, Square end)
        {

            Piece mainPiece = board.GetPiece(start);
            Piece targetSquare = board.GetPiece(end);


            if (board.InBounds(end.Row, end.Col))
            {
                if (targetSquare.IsEmpty)
                {
                    if (mainPiece.IsValidMove(start, end, board)) //if this square can be accessed 
                    {
                        //check if im in check and if i am, check if the next move blocks the check, if not - cant do
                        //go here
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
