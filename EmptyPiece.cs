using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class EmptyPiece : Piece
    {

        public EmptyPiece(Square position) : base(PieceColor.None, PieceType.Empty, position) { }

        public override bool IsValidMove(Square startPoint, Square endPoint, Board board)
        {
            return false;
        }
    }
}
