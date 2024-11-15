using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Bishop : Piece
    {


        public Bishop(PieceColor color, Position position) : base(color, PieceType.Bishop, position) { }



        public override bool IsValidMove(Position startPoint, Position endPoint, Board board)
        {
            int rowDiffernce = Math.Abs(endPoint.Row - startPoint.Row);
            int colDiffernce = Math.Abs(endPoint.Col - startPoint.Col);

            if (rowDiffernce != colDiffernce || rowDiffernce == 0 || colDiffernce == 0)
            {
                return false;
            }

            else //assuming the board is empty
            {
                return true;
            }




            /*for (int times = 0; times < rowDiffernce; times++)
            {
                if(endPoint.Row < startPoint.Row) //up
                {
                    if (endPoint.Col < startPoint.Col)//left
                    {

                    }
                    else//right
                    {

                    }
                }
                else // down
                {
                    if (endPoint.Col > startPoint.Col)// right
                    {

                    }
                    else//left
                    {

                    }
                }
            }*/
        }








        //this logic should be in the game, a recurssion to check if the path is cleared to the point i want to move to

/*
        public bool IsPathClear(int difference, Position start, bool isUp, bool isRight)
        {

            if(difference == 0)
            {
                return true;
            }

            for(int times = 1; times <= difference; times++)
            {
                if(isUp && isRight)
                {


                    // cols + time         rows -time
                }
                else if(isUp && !isRight)
                {
                    // col and row - times
                }
                else if(!isUp && isRight)
                {
                    // cols and rows + times
                }
                else // !isUp && !isRight
                {
                    // cols - times        rows + times
                }

            }
        }


        public bool IsPathClear(int difference, Position start, Position end)
        {

            if (difference == 0)
            {
                return true;
            }

            for (int times = 1; times <= difference; times++)
            {
                if (end.Row < start.Row) //up
                {
                    if (end.Col < start.Col)//left up
                    {
                        // col and row - times
                    }
                    else//right up
                    {
                        // cols + time         rows -time
                    }
                }
                else // down
                {
                    if (end.Col > start.Col)// right down
                    {
                        // cols and rows + times
                    }
                    else//left down
                    {
                        // cols - times        rows + times
                    }
                }
            }

            if ()
        }
*/

    }
}
