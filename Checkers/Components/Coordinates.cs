namespace Checkers.Components
{
    public struct Coordinates 
    {
        // member fields
        private int m_Y;
        private int m_X;

        /* 
         * Coordinates constructor for initilize the coordinate.
         **/
        public Coordinates(int i_X, int i_Y) 
        {
            m_Y = i_Y;
            m_X = i_X;
        }

        //------------------------------------Proporties--------------------------------------------

        /*
         * Get or set the y coordinate
         */
        public int Y
        {
            get
            {
                return m_Y;
            }

            set
            {
                m_Y = value;
            }
        }

        /*
         * Get or set the x coordinate
         */
        public int X
        {
            get
            {
                return m_X;
            }

            set
            {
                m_X = value;
            }
        }

        //------------------------------Other functions------------------------------------

        /* 
         * EqualsTo is a method that check if this coordinate equals to the given coordinate.
         */
        public bool EqualsTo(Coordinates i_Other)
        {
            return m_X == i_Other.X && m_Y == i_Other.Y;
        }
    }
}