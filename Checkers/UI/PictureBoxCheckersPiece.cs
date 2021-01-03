using System.Windows.Forms;
using Checkers.Enums;

namespace Checkers.UI
{
    public class PictureBoxCheckersPiece : PictureBox
    {
        // member fields
        private ButtonSquare m_BelowSquare = null;
        private eTeam m_CheckersPieceTeam;

        /*
        *  PictureBoxCheckersPiece constructor.
        */
        public PictureBoxCheckersPiece(eTeam i_CheckersPieceTeam)
        {
            m_CheckersPieceTeam = i_CheckersPieceTeam;
        }

        /*
         * Get and Set the button square that below piece.
         */
        public ButtonSquare BelowSquare
        {
            get
            {
                return m_BelowSquare;
            }

            set
            {
                m_BelowSquare = value;
                this.Location = m_BelowSquare.Location;
            }
        }

        /*
         * Get CheckersPieceTeam.
         */
        public eTeam CheckersPieceTeam
        {
            get
            {
                return m_CheckersPieceTeam;
            }
        }
    }
}
