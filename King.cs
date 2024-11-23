using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class King : Piece
    {
        public King(PieceColor color, Square position) : base(color, PieceType.King, position) { }



        public override bool IsValidMove(Square startPoint, Square endPoint, Board board)
        {
            int rowDiffernce = Math.Abs(endPoint.Row - startPoint.Row);
            int colDiffernce = Math.Abs(endPoint.Col - startPoint.Col);

            if (rowDiffernce + colDiffernce > 2)
            {
                return false;
            }
            else if (rowDiffernce == 1 && colDiffernce == 1)
            {
                return true;
            }
            else if (rowDiffernce == 1 && colDiffernce == 0)
            {
                return true;
            }
            else if (rowDiffernce == 0 && colDiffernce == 1)
            {
                return true;
            }

            return false;

        }




    }
}
