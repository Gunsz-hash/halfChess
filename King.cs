using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class King : Piece
    {
        public King(PieceColor color, Position position) : base(color, PieceType.Knight, position) { }



        public override bool IsValidMove(Position startPoint, Position endPoint, Board board)
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


        // function that checks whether if king is checked 
        // function that checks if in the next move the king is still checked and if so reverse the move





    }
}
