namespace Checkers.Components
{
    public class GameBoard
    {
        // readonly class constant
        public readonly byte r_Size;

        // member field
        private Piece[,] m_Board;

        /*
         *  GameBoard constructor for initilize the game board.
         */
        public GameBoard(byte i_BoardSize)
        {
            r_Size = i_BoardSize;
            m_Board = new Piece[r_Size, r_Size];
        }

        //------------------------------------Proporties--------------------------------------------
        /*
         *  This indexer get or set Piece in the place of the cell board that the coordinate represent.
         *  remarks: The piece that return can be null if there is not any piece at the given coordinate.
         */
        public Piece this[Coordinates i_Cell]
        {
            get
            {
                return m_Board[i_Cell.Y, i_Cell.X];
            }

            set
            {
                m_Board[i_Cell.Y, i_Cell.X] = value;
            }
        }
        
        //-------------------------------------Public funcations---------------------------------------
        
        /*
         * DoesCellExist is a method that return true if the given cell coordinate is exist coordinate at game board.
         */
        public bool DoesCellExist(Coordinates i_Cell)
        {
            return i_Cell.Y >= 0 && i_Cell.Y < r_Size && i_Cell.X >= 0 && i_Cell.X < r_Size;
        }

        /*
         * IsOccupiedCell is a method that receive cell coordinate and return true if there is piece in this cell at board.
        */ 
        public bool IsOccupiedCell(Coordinates i_Cell)
        {
            return m_Board[i_Cell.Y, i_Cell.X] != null;
        }

        /*
         * ClearCell is a method that receive coordinate and clear from game board the piece in this place (if it exist).
         */
        public void ClearCell(Coordinates i_Cell)
        {
            m_Board[i_Cell.Y, i_Cell.X] = null;
        }

        /*
         * ClearBoard is a method that clear the board from any piece
         */
        public void ClearBoard()
        {
            foreach (Piece currentPiece in m_Board)
            {
                if (currentPiece != null)
                {
                    ClearCell(currentPiece.CoordinatesOnBoard);
                }
            }
        }
    }
}