using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Pawn : Piece
    {


        public Pawn(PieceColor color, Square position) : base(color, PieceType.Pawn, position) { }

        public override bool IsValidMove(Square startPoint, Square endPoint, Board board)
        {

            int rowDiffernce = Math.Abs(endPoint.Row - startPoint.Row);
            int colDiffernce = Math.Abs(endPoint.Col - startPoint.Col);


            //need to check for a piece(if its a capture or a move:
            Piece target = board.GetPiece(endPoint);


            if(!target.IsEmpty) // if its a capture (diagnonally)
            {
                if(rowDiffernce == 1 && colDiffernce == 1)
                {
                    return CheckUpOrDown(startPoint, endPoint);
                }
                return false;
            }

            // a move:



            //if too long
            if (rowDiffernce + colDiffernce > 2)
            {
                return false;
            }


            // forward moves (also backwards, but disabled)

       

            else if ((rowDiffernce == 0 && colDiffernce == 0) || rowDiffernce > 2 || colDiffernce > 1) // (start and end are the same) or if more than 2 rows or 1 col
            {
                return false;
            }




            else if (rowDiffernce > 1 && colDiffernce == 0) //if i can use a double "jump"
            {
                if (this.Color == PieceColor.Black)
                {
                    if (this.Position.Row != 1)
                    {
                        return false;
                    }

                    else
                    {
                        return CheckUpOrDown(startPoint, endPoint); ;
                    }

                }
                else
                {
                    if (this.Position.Row != 6)
                    {
                        return false;
                    }

                    else
                    {
                        return CheckUpOrDown(startPoint, endPoint); ;
                    }
                }


            }

            else if (rowDiffernce == 1 && colDiffernce == 0) //regular forward move
            {
                return CheckUpOrDown(startPoint, endPoint);
            }

            // horizontal moves

            else if (rowDiffernce == 0 && colDiffernce == 1)
            {
                return true;
            }

            
            else
            {
                return false;
            }

        }

        public bool CheckUpOrDown(Square start, Square end)
        {
            if (this.Color == PieceColor.Black) // down
            {
                if (end.Row > start.Row)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else
            {
                if (end.Row < start.Row) //up
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
}
