using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Board
    {
        private Piece[,] squares;

        public const int Rows = 8;
        public const int Columns = 4;


        public Board(int rows, int cols)
        {
            squares = new Piece[rows, cols];
            InitBoard();
        }
        public Board() 
        {
            squares = new Piece[Rows,Columns];
            InitBoard();

        }



        public void InitBoard() //todo change all the hardcoded numbers to consts.
        {
            //init the black pieces
            SetPiece(new Square(0, 0), new King(PieceColor.Black, new Square(0, 0)));
            SetPiece(new Square(0, 1), new Bishop(PieceColor.Black, new Square(0, 1)));
            SetPiece(new Square(0, 2), new Knight(PieceColor.Black, new Square(0, 2)));
            SetPiece(new Square(0, 3), new Rook(PieceColor.Black, new Square(0, 3)));


            //init the black&white pawns
            for (int col = 0; col < Columns; col++)
            {
                SetPiece(new Square(1, col), new Pawn(PieceColor.Black), new Square(1, col));
                SetPiece(new Square(6, col), new Pawn(PieceColor.White), new Square(6, col));
            }

            //init the white pieces
            SetPiece(new Square(7, 0), new King(PieceColor.White, new Square(7, 0)));
            SetPiece(new Square(7, 1), new Bishop(PieceColor.White, new Square(7, 1)));
            SetPiece(new Square(7, 2), new Knight(PieceColor.White, new Square(7, 2)));
            SetPiece(new Square(7, 3), new Rook(PieceColor.White, new Square(7, 3)));

        }
        public bool InBounds(int Row, int Col)
        {
            return Row >= 0 && Row < Rows && Col >= 0 && Col < Columns;
        }

        public void SetPiece(Square position, Piece piece)
        {
            if(InBounds(position.Row, position.Col))
            {
                squares[position.Row, position.Col] = piece;
                //if(piece != null)  // might be, todo
                piece.SetPosition(position);
            }
        }

        public Piece GetPiece(Square position)
        {
            //if(position.inBound();
            return squares[position.Row, position.Col];

            //return null
        }

        public bool MovePiece(Square startPoint, Square endPoint)
        { 


            Piece piece = GetPiece(startPoint);
            if(piece != null && piece.IsValidMove(startPoint,endPoint,this) && MayMove(piece, endPoint))
            {
                SetPiece(endPoint, piece);
                SetPiece(startPoint, null); // todo null or empty, check later;
                return true;
            }

            else if(piece != null && piece.IsValidMove(startPoint, endPoint, this) && !MayMove(piece, endPoint))
            {
                // print some piece is blocking or friendly piece in endPoint
                return false;
            }
            
            else
            {
                // print one condition of the above is false. add general note

                return false;
            }
        }

        public bool MayMove(Piece piece, Square position)
        {
            // check if empty or friendly piece, or hostile piece, todo
        }

        //set piece for moving
       /* public void SetPiece(Position newPosition, Piece piece) //todo object initializer
        {
            if(newPosition.InBounds(Rows, Columns))
            {
                //copying the old pos
                Position oldPosition = new Position(piece.Position.Row, piece.Position.Col);

                //setting the new piece in the new location and changing its position
                squares[newPosition.Row, newPosition.Col] = piece;
                piece.SetPosition(newPosition);

                //set the old position on the board empty
                squares[oldPosition.Row, oldPosition.Col] = new EmptyPiece(oldPosition);



               *//* //todo change it into a function, add a way to copy;
                Position temp = new Position(piece.Position.Row, piece.Position.Col);
                Piece piece1 = new Piece();

                squares[position.Row, position.Col] = piece;
                squares[temp.Row, temp.Col] = piece
                //might be a bug below, define rempty piece*//*

            }
        }*/
    }
}
