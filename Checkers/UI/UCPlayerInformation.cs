using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers.UI
{
    [ToolboxItem(true)]
    public partial class UCPlayerInformation : UserControl
    {
        // member fields
        private readonly Color r_EmphasizeColor = Color.Blue;
        private readonly Color r_DeEmphasizeColor = Color.Black;
        private bool m_IsMyTurn = false;

        /*
        *  UCPlayerInformation constructor.
        */
        public UCPlayerInformation()
        {
            InitializeComponent();
        }

        /*
         * Set the GroupPicture.
         */
        public string GroupPicture
        {
            set
            {
                pictureBoxColor.ImageLocation = value;
            }
        }

        /**
         * Get and sets text in name label
         */
        public string PlayerName
        {
            get
            {
                return labelPlayerName.Text;
            }

            set
            {
                labelPlayerName.Text = value + ":";
            }
        }

        /**
        * Get and sets text in score label
        */
        public string PlayerScore
        {
            get
            {
                return labelPlayerScore.Text;
            }

            set
            {
                labelPlayerScore.Text = value;
            }
        }

        /**
        * Get and sets IsMyTurn
        */
        public bool IsMyTurn
        {
            get { return m_IsMyTurn; }
            set { m_IsMyTurn = value; }
        }


        /**
         * Emphasize is a method that emphasize player name and score of player information uc.
         */
        public void Emphasize()
        {
            labelPlayerName.Font = new Font(labelPlayerName.Font, FontStyle.Bold); 
            labelPlayerScore.Font = new Font(labelPlayerScore.Font, FontStyle.Bold);
            labelPlayerName.ForeColor = r_EmphasizeColor;
            labelPlayerScore.ForeColor = r_EmphasizeColor;
        }

        /**
         * Deemphasize is a method that deemphasize player name and score of player information uc
         */
        public void DeEmphasize()
        {
            labelPlayerName.Font = new Font(labelPlayerName.Font, FontStyle.Regular);
            labelPlayerScore.Font = new Font(labelPlayerScore.Font, FontStyle.Regular);
            labelPlayerName.ForeColor = r_DeEmphasizeColor;
            labelPlayerScore.ForeColor = r_DeEmphasizeColor;
        }
    }
}