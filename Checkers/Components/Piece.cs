using Checkers.Enums;

namespace Checkers.Components
{
    public class Piece
    {
        // readonly class constant
        public readonly eTeam r_Team;

        // member fields
        private bool m_IsKing = false;
        private eSymbol m_Symbol;
        private Coordinates m_CoordinatesOnBoard;

        /*
        *  Piece constructor for initilize the piece.
        */
        public Piece(eTeam i_Team, Coordinates i_Coordinates)
        {
            r_Team = i_Team;
            m_Symbol = i_Team == eTeam.Black ? eSymbol.BlackPiece : eSymbol.WhitePiece;
            m_CoordinatesOnBoard = i_Coordinates;
        }


        //------------------------------------Proporties--------------------------------------------

        /*
         * Get or set the this piece coordinate on board
        */
        public Coordinates CoordinatesOnBoard
        {
            get
            {
                return m_CoordinatesOnBoard;
            }

            set
            {
                m_CoordinatesOnBoard = value;
            }
        }

        /*
        * Get if this piece is a king piece
        */
        public bool IsKing
        {
            get
            {
                return m_IsKing;
            }
        }

        /*
        * Get this piece symbol
        */
        public eSymbol Symbol
        {
            get
            {
                return m_Symbol;
            }
        }

        //---------------------------------Public class method------------------------------------

        /* 
         *  MakeKing is a method that check if this piece is not a king already, and if not make him king
         */
        public void MakeKing()
        {
            if (!m_IsKing)
            {
                m_IsKing = true;
                m_Symbol = m_Symbol == eSymbol.WhitePiece ? eSymbol.WhiteKing : eSymbol.BlackKing;
            }
        }
    }
}