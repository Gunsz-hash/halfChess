using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Rook : Piece
    {
        public Rook(PieceColor color, Position position) : base(color, PieceType.Knight, position) { }

        public override bool IsValidMove(Position startPoint, Position endPoint, Board board)
        {
            int rowDiffernce = Math.Abs(endPoint.Row - startPoint.Row);
            int colDiffernce = Math.Abs(endPoint.Col - startPoint.Col);
            
            
            if ((rowDiffernce != 0 && colDiffernce == 0) || (colDiffernce != 0 && rowDiffernce == 0))
            {
                return true;
            }

            else // row and cols are changes
            {
                return false;
            }
        }


        /*
         
        right : startPoint.col < endPoint.col ( row stays the same)
        
        left  : startPoint.col > endPoint.col ( row stays the same)
         
         
        up : startPoint.row > endPoint.row ( cols stays the same)

        down : startPoint.row < endPoint.row ( cols stays the same)
         
         // consider a piece blocking our rook
         
         
         */








    }
}
