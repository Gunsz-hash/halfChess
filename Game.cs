using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Game
    {
            
        private Board board;
        private bool isWhiteTurn;

        public Game()
        {
            board = new Board();
            isWhiteTurn = true;
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

        




    }
}
