using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Knight : Piece
    {
        public Knight(PieceColor color, Square position) : base(color, PieceType.Knight, position) { }



        public override bool IsValidMove(Square startPoint, Square endPoint, Board board)
        {
            int rowDiffernce = Math.Abs(endPoint.Row - startPoint.Row);
            int colDiffernce = Math.Abs(endPoint.Col - startPoint.Col);

            if ((rowDiffernce==2 && colDiffernce==1))
            {
                return true;
            }
            else if ((rowDiffernce == 1 && colDiffernce == 2))
            {
                return true;

            }
            else
            {
                return false;
            }                   

        }

       

    }
}

