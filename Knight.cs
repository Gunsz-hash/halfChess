using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Knight : Piece
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

        //public bool CheckDirectionRow(Position start, Position end)   /// logic
        //{

        //    // up & right 

        //    if (start.Row > end.Row && start.Col + 1 == end.Col)
        //    {
        //        return true;
        //    }


        //    // right & up      

        //    else if (start.Row > end.Row && start.Col + 2 == end.Col)
        //    {
        //        return true;
        //    }




        //    // up & left

        //    else if (start.Row > end.Row && start.Col - 1 == end.Col)
        //    {
        //        return true;
        //    }

        //    // left & up

        //    else if (start.Row > end.Row && start.Col - 2 == end.Col)
        //    {

        //        return true;

        //    }

        //    // down & right 

        //    else if (start.Row < end.Row && start.Col + 1 == end.Col)
        //    {
        //        return true;
        //    }


        //    // right & down      

        //   else if (start.Row < end.Row && start.Col + 2 == end.Col)
        //    {
        //        return true;
        //    }


        //    // down & left

        //    else if (start.Row < end.Row && start.Col - 1 == end.Col)
        //    {
        //        return true;
        //    }

        //    // left & down

        //    else if (start.Row < end.Row && start.Col - 2 == end.Col)
        //    {

        //        return true;

        //    }


        //    return false;


        //}


    }
}

