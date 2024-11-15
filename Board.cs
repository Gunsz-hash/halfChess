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
        
        
        public Board(int rows, int cols)
        {
            squares = new Piece[rows, cols];
            InitBoard();
        }

        public void InitBoard() //todo change all the hardcoded numbers to consts.
        {
            //init the black pieces
            SetPiece(new Position(0, 0), new King(PieceColor.Black, new Position(0, 0)));
            SetPiece(new Position(0, 1), new Bishop(PieceColor.Black, new Position(0, 1)));
            SetPiece(new Position(0, 2), new Knight(PieceColor.Black, new Position(0, 2)));
            SetPiece(new Position(0, 3), new Rook(PieceColor.Black, new Position(0, 3)));


            //init the black&white pawns
            for (int col = 0; col < Columns; col++)
            {
                SetPiece(new Position(1, col), new Pawn(PieceColor.Black), new Position(1, col));
                SetPiece(new Position(6, col), new Pawn(PieceColor.White), new Position(6, col));
            }

            //init the white pieces
            SetPiece(new Position(7, 0), new King(PieceColor.White, new Position(7, 0)));
            SetPiece(new Position(7, 1), new Bishop(PieceColor.White, new Position(7, 1)));
            SetPiece(new Position(7, 2), new Knight(PieceColor.White, new Position(7, 2)));
            SetPiece(new Position(7, 3), new Rook(PieceColor.White, new Position(7, 3)));

        }

        public void SetPiece(Position position, Piece piece)
        {
            if(position.InBounds(Rows,Columns))
            {
                squares[position.Row, position.Col] = piece;
                //if(piece != null)  // might be, todo
                piece.SetPosition(position);
            }
        }

        public Piece GetPiece(Position position)
        {
            //if(position.inBound();
            return squares[position.Row, position.Col];

            //return null
        }

        public bool MovePiece(Position startPoint, Position endPoint)
        {
            Piece piece = GetPiece(startPoint);
            if(piece != null && piece.IsValidMove() && MayMove(piece, endPoint))
            {
                SetPiece(endPoint, piece);
                SetPiece(startPoint, null); // todo null or empty, check later;
            }
            else
            {
                return false;
            }
        }

        public bool MayMove(Piece piece, Position position)
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
