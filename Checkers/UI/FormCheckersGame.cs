using Checkers.Components;
using Checkers.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers.UI
{
    public partial class FormCheckersGame : Form
    {
        // member field
        private PictureBoxCheckersPiece m_CurrentSelectedPiece = null;

        /*
        *  FormCheckersGame constructor for initilize the checker game form
        */
        public FormCheckersGame(byte i_CheckersBoardSize)
        {
            r_BoardSize = i_CheckersBoardSize;
            r_NumberOfPiecesInOneSide = (byte)((r_BoardSize / 2) * ((r_BoardSize / 2) - 1));

            buttonSquares = new ButtonSquare[r_BoardSize, r_BoardSize];
            pictureBoxCheckersPieceWhitePieces = new PictureBoxCheckersPiece[r_NumberOfPiecesInOneSide];
            pictureBoxCheckersPieceBlackPieces = new PictureBoxCheckersPiece[r_NumberOfPiecesInOneSide];

            InitializeComponent();

            initializePiecesComponent();
            initializeButtonsComponent();
            initializeLocationOfPieces();

            resizeClient(); 
            addControlsButtonSquareToForm();
            ucPlayer1Information.IsMyTurn = true;
            EmphasizeCurrentPlayer();
            addGroupPictureToUIPlayersInformation();
        }

        //------------------------------------Proporties--------------------------------------------

        /*
         * Set Player1 Name.
         */
        public string Player1Name
        {
            set
            {
                ucPlayer1Information.PlayerName = value;
            }
        }

        /*
         * Set Player2 Name.
         */
        public string Player2Name
        {
            set
            {
                ucPlayer2Information.PlayerName = value;
            }
        }

        /*
         * Set Player1 Score.
         */
        public string ScorePlayer1
        {
            set
            {
                ucPlayer1Information.PlayerScore = value;
            }
        }

        /*
         * Set Player2 Score.
         */
        public string ScorePlayer2
        {
            set
            {
                ucPlayer2Information.PlayerScore = value;
            }
        }

        /*
         * Get the current selected piece.
         */
        public PictureBoxCheckersPiece CurrentSelectedPiece
        {
            get
            {
                return m_CurrentSelectedPiece;
            }
        }

        //------------------------------Public functions-----------------------------------

        /*
         * MoveCurrentSelectedPiece is a method that moves the piece selected to the given
         * row and column location.
         */
        public void MoveCurrentSelectedPiece(int i_RowOfNewSquare, int i_ColumnOfNewSquare)
        {
            if (m_CurrentSelectedPiece != null)
            {
                m_CurrentSelectedPiece.BelowSquare = buttonSquares[i_RowOfNewSquare, i_ColumnOfNewSquare]; // The new square that under this piece
                pieceSelection(m_CurrentSelectedPiece);
            }
        }

        /*
         *  MakeCurrentSelectedPieceKing is a method that turns the selected piece into a king.
         */
        public void MakeCurrentSelectedPieceKing()
        {
            if (m_CurrentSelectedPiece.CheckersPieceTeam == eTeam.White)
            {
                m_CurrentSelectedPiece.ImageLocation = @"..\..\..\Images\WhitePieaceKing.gif";
            }
            else
            {
                m_CurrentSelectedPiece.ImageLocation = @"..\..\..\Images\BlackPieaceKing.gif";
            }
        }

        /*
         *  EatPieceThatOnButtonSquare is a method that remove the piece from the square in the given row and column.
         */
        public void EatPieceThatOnButtonSquare(int i_SquareBelowPieceRow, int i_SquareBelowPieceColumn, eTeam i_Team)
        {
            if (i_Team == eTeam.White)
            {
                removePieceOnSquare(i_SquareBelowPieceRow, i_SquareBelowPieceColumn, pictureBoxCheckersPieceWhitePieces);
            }
            else
            {
                removePieceOnSquare(i_SquareBelowPieceRow, i_SquareBelowPieceColumn, pictureBoxCheckersPieceBlackPieces);
            }
        }

        /*
         *  SignDelegateToAllSquareEvent is a method that sing the given event handler to square click event.
         */
        public void SignDelegateToAllSquareEvent(EventHandler i_ButtonSquare_Clicked)
        {
            foreach (ButtonSquare square in buttonSquares)
            {
                square.Click += new EventHandler(i_ButtonSquare_Clicked);
            }
        }

        /*
         *  ResetFormToStartingPosition is a method that resets the 
         *  form that represents the game board into its original condition.
         */
        public void ResetFormToStartingCondition()
        {
            for (int i = 0; i < r_NumberOfPiecesInOneSide; i++)
            {
                // Delete left over
                if(pictureBoxCheckersPieceWhitePieces[i] != null)
                {
                    Controls.Remove(pictureBoxCheckersPieceWhitePieces[i]);
                }
                else if (pictureBoxCheckersPieceBlackPieces[i] != null)
                {
                    Controls.Remove(pictureBoxCheckersPieceBlackPieces[i]);
                }
            }

            pieceSelection(m_CurrentSelectedPiece); // If there any piece that selected it will be null

            initializePiecesComponent();
            initializeLocationOfPieces();
            emphasizeAndDeEmphasizeUserControl(ucPlayer1Information, ucPlayer2Information);
        }

        /*
         * EmphasizeCurrentPlayer is a method that emphasizes the current player that it is his turn.
         */
        public void EmphasizeCurrentPlayer()
        {
            if(ucPlayer1Information.IsMyTurn)
            {
                emphasizeAndDeEmphasizeUserControl(ucPlayer1Information, ucPlayer2Information);
            }
            else
            {
                emphasizeAndDeEmphasizeUserControl(ucPlayer2Information, ucPlayer1Information);
            }
        }

        /*
         *  ChangePlayerTurn is a method that change player turn 
         *  and handle the necessary visual change accordingly 
         */
        public void ChangePlayerTurn()
        {
            if(ucPlayer1Information.IsMyTurn)
            {
                ucPlayer1Information.IsMyTurn = false;
                ucPlayer2Information.IsMyTurn = true;
            }
            else
            {
                ucPlayer1Information.IsMyTurn = true;
                ucPlayer2Information.IsMyTurn = false;
            }
            EmphasizeCurrentPlayer();
        }

        /*
         *  CleanSelection is a method that clean selection current piece visually. 
         */
        public void CleanSelection()
        {
            pieceSelection(m_CurrentSelectedPiece);
        }

        /*
         *  IsAnyPieceSelected is a method that return true if any piece selected.
         */
        public bool IsAnyPieceSelected()
        {
            return m_CurrentSelectedPiece != null;
        }

        /*
         *  SelectPieceByCoordinate is a method that receive row,column and team and select the piece
         *  that is in the given row and column from the relevant collection of pieces.
         */
        public void SelectPieceByCoordinate(int i_Row, int i_Column, eTeam i_Team)
        {
            PictureBoxCheckersPiece pieceToSelect;

            if (i_Team == eTeam.White)
            {
                pieceToSelect = getPieceByCoordinates(i_Row, i_Column, pictureBoxCheckersPieceWhitePieces);
            }
            else
            {
                pieceToSelect = getPieceByCoordinates(i_Row, i_Column, pictureBoxCheckersPieceBlackPieces);
            }

            pieceSelection(pieceToSelect);
        }

        //----------------------------------Private functions--------------------------------------

        /// ----------------------------------Event handler------------------------------------

        /*
         * piece_Clicked is an event handler method that will execute when piece is clicked. 
         */
        private void piece_Clicked(object sender, EventArgs e)
        {
            pieceSelection(sender);
        }

        /// -----------------------------Remove & Select function------------------------------

        /*
         *  removePieceOnSquare is a method that recieve row and column of square below piece that
         *  need to be remove, in addition recieve collection of pieces and remove the relevant piece
         *  from the collection.
         */
        private void removePieceOnSquare(int i_SquareBelowPieceRow, int i_SquareBelowPieceColmen, PictureBoxCheckersPiece[] i_Pieces)
        {
            ButtonSquare belowButtonSquare = buttonSquares[i_SquareBelowPieceRow, i_SquareBelowPieceColmen];

            // scan array and locate the piece whose cell coordinates are received in function:
            for (int i = 0; i < r_NumberOfPiecesInOneSide; i++)
            {
                if (i_Pieces[i] != null)
                {
                    // Check if the current piece it what we looking for
                    if (i_Pieces[i].BelowSquare == belowButtonSquare)
                    {
                        Controls.Remove(i_Pieces[i]);
                        i_Pieces[i] = null;
                        break;
                    }
                }
            }
        }

        /*
         *  pieceSelection is a method that select the given piece (if it recive a valid piece).
         */
        private void pieceSelection(object i_PieceSender)
        {
            PictureBoxCheckersPiece selectedPiece = i_PieceSender as PictureBoxCheckersPiece;

            if (IsAnyPieceSelected())
            {
                m_CurrentSelectedPiece.BackColor = Color.White; // Return the color of previous selected piece to white.

                if (m_CurrentSelectedPiece != selectedPiece)
                {
                    m_CurrentSelectedPiece = selectedPiece;        // Now m_CurrentSelectedPiece is selectedPiece.
                    m_CurrentSelectedPiece.BackColor = Color.DodgerBlue;
                }
                else
                {
                    m_CurrentSelectedPiece = null;  // If we select this piece before it means that now we cancel the select.
                }
            }
            else
            {
                if (selectedPiece != null)
                {
                    m_CurrentSelectedPiece = selectedPiece;
                    m_CurrentSelectedPiece.BackColor = Color.DodgerBlue;
                }
            }
        }

        /// ----------------------------------Other Functions---------------------------------
        /// 
        /*
         *  getPieceByCoordinates is a method that return the piece that is at the given coordinate.
         */
        private PictureBoxCheckersPiece getPieceByCoordinates(int i_SquareBelowPieceRow, int i_SquareBelowPieceColmen, PictureBoxCheckersPiece[] i_Pieces)
        {
            ButtonSquare belowButtonSquare = buttonSquares[i_SquareBelowPieceRow, i_SquareBelowPieceColmen];
            PictureBoxCheckersPiece pieceToSelect = null;

            foreach (PictureBoxCheckersPiece piece in i_Pieces)
            {
                if (piece != null)
                {
                    if (piece.BelowSquare == belowButtonSquare)
                    {
                        pieceToSelect = piece;
                        break;
                    }
                }
            }

            return pieceToSelect;
        }

        /*
         *  emphasizeAndDeEmphasizeUserControl is a method that receive UCPlayerInformation to emphasize and emphasize it,
         *  and receive UCPlayerInformation to deemphasize and deemphasize it.
         */
        private void emphasizeAndDeEmphasizeUserControl(UCPlayerInformation i_ToEmphasize, UCPlayerInformation i_ToDeEmphasize)
        {
            i_ToEmphasize.Emphasize();
            i_ToDeEmphasize.DeEmphasize();
        }

        /// --------------------------Initialization Functions From-------------------------

        /*
         *  initializeButtonsComponent is a method that initialize the buttons components.
         */
        private void initializeButtonsComponent()
        {
            // Coordinates for the graphic location:
            int xCoordinate = (int)eSizeAndLocations.ButtonXStartPosition;
            int yCoordinate = (int)eSizeAndLocations.ButtonYStartPosition;

            for (byte row = 0; row < r_BoardSize; row++)
            {
                for (byte column = 0; column < r_BoardSize; column++)
                {
                    buttonSquares[row, column] = new ButtonSquare(row, column);
                    buttonSquares[row, column].LocationInBoard = new Coordinates(column, row);

                    // Check if the current buttonSquare is black or white
                    if (((column % 2) == 0 && (row % 2) == 0) || ((column % 2) != 0 && (row % 2) != 0))
                    {
                        buttonSquares[row, column].BackColor = System.Drawing.Color.Black;
                        buttonSquares[row, column].Enabled = false;
                    }
                    else
                    {
                        buttonSquares[row, column].BackColor = System.Drawing.Color.White;
                    }

                    this.buttonSquares[row, column].Location = new System.Drawing.Point(xCoordinate, yCoordinate);
                    this.buttonSquares[row, column].Size = new System.Drawing.Size((int)eSizeAndLocations.ButtonSize, (int)eSizeAndLocations.ButtonSize);
                    this.buttonSquares[row, column].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    xCoordinate += (int)eSizeAndLocations.ButtonSize;
                }

                yCoordinate += (int)eSizeAndLocations.ButtonSize;
                xCoordinate = (int)eSizeAndLocations.ButtonXStartPosition;
            }
        }

        /*
         *  initializePiecesComponent is a method that initialize the pieces components.
         */
        private void initializePiecesComponent()
        {
            for (int i = 0; i < r_NumberOfPiecesInOneSide; i++)
            {
                // Initialize white pieces
                pictureBoxCheckersPieceWhitePieces[i] = new PictureBoxCheckersPiece(eTeam.White);
                pictureBoxCheckersPieceWhitePieces[i].ImageLocation = @"..\..\..\Images\WhitePieace.gif";
                initializePictureBoxCheckersPiece(pictureBoxCheckersPieceWhitePieces[i]);

                // Initialize black pieces
                pictureBoxCheckersPieceBlackPieces[i] = new PictureBoxCheckersPiece(eTeam.Black);
                pictureBoxCheckersPieceBlackPieces[i].ImageLocation = @"..\..\..\Images\BlackPieace.gif";
                initializePictureBoxCheckersPiece(pictureBoxCheckersPieceBlackPieces[i]);
            }
        }

        /*
         *  initializePictureBoxCheckersPiece is a method that receive pictureBoxCheckersPiece component and initialize it.
         */
        private void initializePictureBoxCheckersPiece(PictureBoxCheckersPiece i_PictureBoxCheckersPiece)
        {
            i_PictureBoxCheckersPiece.Size = new System.Drawing.Size(50, 50);
            i_PictureBoxCheckersPiece.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            i_PictureBoxCheckersPiece.BackColor = System.Drawing.Color.White;
            this.Controls.Add(i_PictureBoxCheckersPiece);
            i_PictureBoxCheckersPiece.Click += new EventHandler(this.piece_Clicked);
        }

        /*
         *  initializeLocationOfPieces is a method that initialize the location of pieces.
         */
        private void initializeLocationOfPieces()
        {
            initializeLocationOneSideOfPieces(0, (r_BoardSize / 2) - 1, pictureBoxCheckersPieceWhitePieces);
            initializeLocationOneSideOfPieces((r_BoardSize / 2) + 1, r_BoardSize, pictureBoxCheckersPieceBlackPieces);
        }

        /*
         *  initializeLocationOneSideOfPieces is a method that initialize the location of one side of pieces.
         */
        private void initializeLocationOneSideOfPieces(int i_StartRow, int i_EndRow, PictureBoxCheckersPiece[] i_Pieces)
        {
            int index = 0;

            for (int row = i_StartRow; row < i_EndRow; row++)
            {
                int startColumn = row % 2 == 0 ? 1 : 0;

                for (int column = startColumn; column < r_BoardSize; column += 2)
                {
                    i_Pieces[index].BelowSquare = buttonSquares[row, column];
                    i_Pieces[index].BringToFront();
                    index++;
                }
            }
        }

        /*
         *  resizeClient is a method that resize the client size according to the size of the board and button size.
         */
        private void resizeClient()
        {
            int clientSizeWidth = ((int)eSizeAndLocations.ButtonSize * r_BoardSize) + (2 * (int)eSizeAndLocations.ButtonXStartPosition);
            int clientSizeHeight = ((int)eSizeAndLocations.ButtonSize * r_BoardSize) + (int)eSizeAndLocations.ButtonYStartPosition + (int)eSizeAndLocations.ButtonXStartPosition;
            ClientSize = new System.Drawing.Size(clientSizeWidth, clientSizeHeight);
        }

        /*
         *  addControlsButtonSquareToForm is a method that adds the constrol button squars to this form.
         */
        private void addControlsButtonSquareToForm()
        {
            foreach (ButtonSquare square in buttonSquares)
            {
                this.Controls.Add(square);
            }
        }

        /*
         *  addColorGroupToPlayersInformation is a method that add group picture to UCPlayerInformation components.
         */
        private void addGroupPictureToUIPlayersInformation()
        {
            ucPlayer1Information.GroupPicture = @"..\..\..\Images\BlackPieace.gif";
            ucPlayer2Information.GroupPicture = @"..\..\..\Images\WhitePieace.gif";
        }

    }
}