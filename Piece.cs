using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public enum PieceType {Empty, King, Rook, Bishop, Knight, Pawn}
    public enum PieceColor {None, White, Black}


    internal abstract class Piece
    {
        public PieceColor Color { get; set; }
        public PieceType Type { get; private set;}
        public Square Position { get; private set;}

        protected Piece(PieceColor color, PieceType type, Square position)
        {
            Color = color;
            Type = type;
            //if(position.InBounds()
            //throw if not;
            Position = position;
        }



        public abstract bool IsValidMove(Square startPoint, Square endPoint, Board board);

        public bool IsWhite()
        {
            return Color == PieceColor.White;
        }

        public void SetPosition(Square newPosition)
        {
            //if(!newPosition.InBounds) todo fix inbound - if not, throw 
            
            //add else clause later;
            Position = newPosition;

        }

       
    }
}
