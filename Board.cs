using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Board
    {
        private Piece[,] squares;

        public const int Rows = 8;
        public const int Columns = 4;
        public King whiteKing { get; set; }
        public King blackKing { get; set; }


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

        /*public IEnumerable<Piece> getPiecesByColor(bool isWhite)
        {
            List<Piece> playerPieces = new List<Piece>();
            for(int row = 0; row <Rows; row++)
            {
                for(int col = 0; col < Columns; col++)
                {
                    Piece piece = piece
                }
            }
        }*/

        public bool IsPathClear(Square start, Square end, bool isDiagonal) //is diagonal used for the bishop problem
        {


            if (isDiagonal)
            {
                int rowDifference = Math.Abs(end.Row - start.Row);
                int colDifference = Math.Abs(end.Col - start.Col);
                if (rowDifference != colDifference)
                {
                    return false;
                }
            }
            // If it's a straight move, make sure it's actually straight
            else
            {
                if (start.Row != end.Row && start.Col != end.Col)
                {
                    return false;
                }
            }



            int rowStep = start.Row == end.Row ? 0 : (end.Row - start.Row) / Math.Abs(end.Row - start.Row);
            int colStep = start.Col == end.Col ? 0 : (end.Col - start.Col) / Math.Abs(end.Col - start.Col);

            int currentRow = start.Row + rowStep;
            int currentCol = start.Col + colStep;


            // Check each square between start and end (exclusive)
            while (currentRow != end.Row || currentCol != end.Col)
            {
                if (!GetPiece(new Square(currentRow, currentCol)).IsEmpty)
                {
                    return false;  // Path is blocked
                }
                currentRow += rowStep;
                currentCol += colStep;
            }
            return true;  // Path is clear
        }

        public void InitBoard() //todo change all the hardcoded numbers to consts.
        {



            // Initialize all squares to empty first
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    squares[row, col] = new EmptyPiece(new Square(row, col));
                }
            }



            //init the black pieces
            blackKing = new King(PieceColor.Black, new Square(0, 0));
            SetPiece(new Square(0, 0), blackKing);
            SetPiece(new Square(0, 1), new Bishop(PieceColor.Black, new Square(0, 1)));
            SetPiece(new Square(0, 2), new Knight(PieceColor.Black, new Square(0, 2)));
            SetPiece(new Square(0, 3), new Rook(PieceColor.Black, new Square(0, 3)));


            //init the black&white pawns
            for (int col = 0; col < Columns; col++)
            {
                SetPiece(new Square(1, col), new Pawn(PieceColor.Black, new Square(1, col)));
                SetPiece(new Square(6, col), new Pawn(PieceColor.White, new Square(6, col)));
            }

            //init the white pieces
            whiteKing = new King(PieceColor.White, new Square(7, 0));
            SetPiece(new Square(7, 0), whiteKing);
            SetPiece(new Square(7, 1), new Bishop(PieceColor.White, new Square(7, 1)));
            SetPiece(new Square(7, 2), new Knight(PieceColor.White, new Square(7, 2)));
            SetPiece(new Square(7, 3), new Rook(PieceColor.White, new Square(7, 3)));

        }
        public bool InBounds(Square position)
        {
            return position.Row >= 0 && position.Row < Rows && position.Col >= 0 && position.Col < Columns;
        }

        public bool InBounds(int Row, int Col)
        {
            return Row >= 0 && Row < Rows && Col >= 0 && Col < Columns;
        }

        public void SetPiece(Square position, Piece piece)
        {
            if(InBounds(position))
            {
                squares[position.Row, position.Col] = piece;
                //if(piece != null)  // might be, todo
                piece.SetPosition(position);
            }
        }

        public Piece GetPiece(Square position)
        {
            if (!InBounds(position))
            {
                return new EmptyPiece(position);
            }

            if (squares[position.Row, position.Col] == null)
            {
                squares[position.Row, position.Col] = new EmptyPiece(position);
            }

            return squares[position.Row, position.Col];
        }

        /* public bool MovePiece(Square startPoint, Square endPoint)
         { 


             Piece piece = GetPiece(startPoint);
             if(!piece.IsEmpty && piece.IsValidMove(startPoint,endPoint,this) && MayMove(piece, endPoint))
             {
                 SetPiece(endPoint, piece);
                 Piece emptyPiece = new EmptyPiece(startPoint);
                 SetPiece(startPoint, emptyPiece);
                 return true;
             }

             else if(!piece.IsEmpty && piece.IsValidMove(startPoint, endPoint, this) && !MayMove(piece, endPoint))
             {
                 // print some piece is blocking or friendly piece in endPoint
                 return false;
             }

             else
             {
                 // print one condition of the above is false. add general note

                 return false;
             }
         }*/

        /*public bool MayMove(Piece piece, Square position)
        {
            // check if empty or friendly piece, or hostile piece, todo
        }*/

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
