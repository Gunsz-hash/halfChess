using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Rook : Piece
    {
        public Rook(PieceColor color, Square position) : base(color, PieceType.Rook, position) { }

        public override bool IsValidMove(Square startPoint, Square endPoint, Board board)
        {
            int rowDifference = Math.Abs(endPoint.Row - startPoint.Row);
            int colDifference = Math.Abs(endPoint.Col - startPoint.Col);

            // Must move either horizontally or vertically (not both)
            if ((rowDifference != 0 && colDifference != 0) || (rowDifference == 0 && colDifference == 0))
            {
                return false;
            }

            // Check if path is clear (false for straight line check)
            return board.IsPathClear(startPoint, endPoint, false);
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
