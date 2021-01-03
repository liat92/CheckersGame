using Checkers.Components;
using System.Windows.Forms;

namespace Checkers.UI
{
    public class ButtonSquare : Button
    {
        // member field
        private Coordinates m_LocationInBoard;

        /*
        *  ButtonSquare constructor.
        */
        public ButtonSquare(int i_Row, int i_Column)
        {
            m_LocationInBoard = new Coordinates(i_Column, i_Row);
        }

        /*
         * Get or set the location in board
         */
        public Coordinates LocationInBoard
        {
            get
            {
                return m_LocationInBoard;
            }

            set
            {
                m_LocationInBoard = value;
            }
        }
    }
}
