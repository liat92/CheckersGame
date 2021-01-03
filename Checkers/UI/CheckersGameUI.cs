using Checkers.Components;
using Checkers.Logic;
using System;
using System.Windows.Forms;

namespace Checkers.UI
{
    public class CheckersGameUI
    {
        // member fields
        private CheckersGame m_CheckersGame = null;
        private FormGameSettings m_FormGameSettings = new FormGameSettings();
        private FormCheckersGame m_FormCheckersGame;

        /*
         * Run is a method that run the checkersGame.
         */
        public void Run()
        {
            initialization();
            if(m_FormCheckersGame != null)
            {
                m_FormCheckersGame.ShowDialog();
            }
        }

        //--------------------------------Private funcations------------------------------------

        /*
         * initialization is a method that initialize CheckersGameUI component.
         */
        private void initialization()
        {
            if (m_FormGameSettings.ShowDialog() == DialogResult.OK)
            {
                // Initialize CheckersGame
                byte boardSize = (byte)m_FormGameSettings.BoardSize;
                string player1Name = m_FormGameSettings.Player1Name;
                string player2Name = m_FormGameSettings.Player2Name;
                bool againstAI = !m_FormGameSettings.CheckBoxPlayer2Checked;
                m_CheckersGame = new CheckersGame(boardSize, player1Name, player2Name, againstAI);
                m_CheckersGame.InitializeGame();
                m_CheckersGame.CellChanged += new CellChangeEventHandler(cell_Changed);
                m_CheckersGame.PieceBecameKing += new PieceBecameKingEventHandler(piece_BecameKing);
                m_CheckersGame.PieceEaten += new PieceEatenEventHandler(piece_Eaten);

                // Initialize FormCheckersGame 
                m_FormCheckersGame = new FormCheckersGame(boardSize);
                m_FormCheckersGame.Player1Name = player1Name;
                m_FormCheckersGame.Player2Name = player2Name;
                m_FormCheckersGame.SignDelegateToAllSquareEvent(buttonSquare_Clicked);
            }
        }

        // ------------------------------------------Event Handler------------------------------------------------------

        /*
         *  piece_BecameKing is an event handler method that will execute when the related piece became king.
         */
        private void piece_BecameKing(CellChangeEventArgs e)
        {
            if (e.PieceBecomeKing)
            {
                m_FormCheckersGame.SelectPieceByCoordinate(e.To.Y, e.To.X, e.Team);
                m_FormCheckersGame.MakeCurrentSelectedPieceKing();
                m_FormCheckersGame.CleanSelection();
            }
        }

        /*
         * piece_Eaten is an event handler method that will execute when the related piece is eaten. 
         */
        private void piece_Eaten(CellChangeEventArgs e)
        {
            m_FormCheckersGame.EatPieceThatOnButtonSquare(e.EatenPiece.Y, e.EatenPiece.X, e.Team);
        }

        /*
         *  buttonSquare_Clicked is an event handler method that will execute when the button square is clicked.
         */
        private void buttonSquare_Clicked(object sender, EventArgs e)
        {
            doPlayerMove(sender as ButtonSquare);

            if(m_CheckersGame.GetCurrentPlayer().IsAI && !m_CheckersGame.IsGameOver())
            {
                m_CheckersGame.ExecuteComputerAction();
                m_FormCheckersGame.ChangePlayerTurn();
            }

            // Check if the game is over
            if (m_CheckersGame.IsGameOver())
            {
                doWhenCurrentGameRoundIsOver();
            }
        }

        /*
         *  cell_Changed is an event handler method that will execute when the related cell is changed.
         */
        private void cell_Changed(CellChangeEventArgs e)
        {
            changeDisplayAccordingToMovement(e);
        }

        // ---------------------------------------general UI methods------------------------------------------------------

        /*
         *  initializeGameRound is a method that initialize the game data to the initialize position.
         *  Note: The score of the players did't change.
         */
        private void initializeGameRound()
        {
            m_CheckersGame.InitializeGame();
            m_FormCheckersGame.ResetFormToStartingCondition();
        }

        /*
         *  doWhenCurrentGameRoundIsOver is a method that handles the end of a game round:
         *  show game result, ask to coninue and close the game if the players not coninue.
         */
        private void doWhenCurrentGameRoundIsOver()
        {
            string resultMessage = getEndOfGameRoundStringMsg();
            DialogResult continueTheGame = MessageBox.Show(resultMessage, "Checkers", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (continueTheGame == DialogResult.Yes)
            {
                m_CheckersGame.UpdatePlayerScore();
                m_FormCheckersGame.ScorePlayer1 = m_CheckersGame.Player1.Score.ToString();
                m_FormCheckersGame.ScorePlayer2 = m_CheckersGame.Player2.Score.ToString();
                initializeGameRound();
            }
            else
            {
                m_FormCheckersGame.Close();
            }
        }

        /*
         *  changeDisplayAccordingToMovement is a method that responsible to update the display according to movement.
         */
        private void changeDisplayAccordingToMovement(CellChangeEventArgs e)
        {
            if (m_CheckersGame.GetCurrentPlayer().IsAI)
            {
                m_FormCheckersGame.SelectPieceByCoordinate(e.From.Y, e.From.X, e.Team);
            }

            m_FormCheckersGame.MoveCurrentSelectedPiece(e.To.Y, e.To.X);
        }

        /*
         *  doPlayerMove is a method that responsible to the execute the player move according to
         *  clicking on the destination button on game board.
         */
        private void doPlayerMove(ButtonSquare i_DestinationButtonSquare)
        {
            if (m_FormCheckersGame.IsAnyPieceSelected())
            {
                Coordinates to = i_DestinationButtonSquare.LocationInBoard;
                Coordinates from = m_FormCheckersGame.CurrentSelectedPiece.BelowSquare.LocationInBoard;

                executePlayerMove(from, to);
            }
        }

        /*
         *  executePlayerMove is a method that responsible to the execute the player move.
         */
        private void executePlayerMove(Coordinates i_From, Coordinates i_To)
        {
            if (m_CheckersGame.IsValidPlayerMove(i_From, i_To))
            {
                bool canDominateAgain;

                m_CheckersGame.ExecutePlayerAction(i_From, i_To, out canDominateAgain);

                if (!canDominateAgain)
                {
                    m_CheckersGame.ChangeTurn();
                    m_FormCheckersGame.ChangePlayerTurn();
                }
            }
            else
            {
                MessageBox.Show("Error: Invalid Move", "Checkers", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-------------------------------Function For MessageBox------------------------------

        /*
         *  getTieOrWinnerString is a method that return Tie or winner string message.
         */
        private string getTieOrWinnerString()
        {
            string tieOrWinnerString;

            // If The winner is null it means that we have a draw:
            if (m_CheckersGame.Winner == null)
            {
                tieOrWinnerString = "Tie";
            }
            else
            {
                tieOrWinnerString = string.Format("The winner is {0}!", m_CheckersGame.Winner.Name);
            }

            return tieOrWinnerString;
        }

        /*
         *  getEndOfGameRoundStringMsg is a method that return a string that will appear after the current game round is over.
         */
        private string getEndOfGameRoundStringMsg()
        {
            string endOfGameRoundStringMsg = string.Format(
@"{0}
Another Round?",
getTieOrWinnerString());

            return endOfGameRoundStringMsg;
        }
    }
}
