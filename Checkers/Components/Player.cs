using System.Collections.Generic;
using Checkers.Enums;

namespace Checkers.Components
{
    public class Player
    {
        // readonly class constant
        public readonly eTeam r_Team;

        // member fields
        private string m_Name = null;
        private int m_Score = 0;
        private bool m_IsAI = false;
        private List<Piece> m_Pieces = new List<Piece>();

        /*
        *  Player constructor for initilize the player.
        */
        public Player(string i_Name, bool i_IsAI, eTeam i_Team)
        {
            m_Name = i_Name;
            m_IsAI = i_IsAI;
            r_Team = i_Team;
            List<Piece> m_Pieces = new List<Piece>();
        }


        //------------------------------------Proporties--------------------------------------------

        /*
        * Get or set the score of this player
        */
        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        /*
        * Get if this player is AI
        */
        public bool IsAI
        {
            get
            {
                return m_IsAI;
            }
        }

        /*
        * Get if this player Name
        */
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /*
        * Get this player pieces
        */
        public List<Piece> Pieces
        {
            get
            {
                return m_Pieces;
            }
        }

        //---------------------------Player's tool functions------------------------------

        /*
         * GetNumberOfPieces is a method that return the number of pieces that the player have
         */
        public int GetNumberOfPieces()
        {
            return m_Pieces.Count;
        }

        /*
         * AddPiece is a method that receive piece and add it to the player pieces
         */
        public void AddPiece(Piece i_Piece)
        {
            m_Pieces.Add(i_Piece);
        }

        /*
         * RemovePiece is a method that receive piece that need to remove 
         * from the player pieces and remove it.
         */
        public void RemovePiece(Piece i_Piece)
        {
            // Verfiy that the array of pieces not empty from pieces
            if (m_Pieces.Count != 0)
            {
                m_Pieces.Remove(i_Piece);
            }
        }

        /*
         * GetNumberOfKings is a method that return the number of kings that the player has.
         */
        public byte GetNumberOfKings()
        {
            // counts number of kings in players pieces list
            byte kingCounter = 0;

            foreach (Piece currentPiece in m_Pieces)
            {
                if (currentPiece.IsKing)
                {
                    kingCounter++;
                }
            }

            return kingCounter;
        }

        /*
         * GetNumberOfRegularPieces is a method that return the number of regular Pieces that the player has.
         */
        public byte GetNumberOfRegularPieces()
        {
            return (byte)(m_Pieces.Count - GetNumberOfKings());
        }
    }
}