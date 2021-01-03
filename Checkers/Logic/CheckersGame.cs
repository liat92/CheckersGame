using System;
using System.Collections.Generic;
using Checkers.Components;
using Checkers.Enums;

namespace Checkers.Logic
{
    // Delegates
    public delegate void CellChangeEventHandler(CellChangeEventArgs e);

    public delegate void PieceBecameKingEventHandler(CellChangeEventArgs e);

    public delegate void PieceEatenEventHandler(CellChangeEventArgs e);

    public class CellChangeEventArgs : EventArgs
    {
        public Coordinates From;
        public Coordinates To;
        public Coordinates EatenPiece;
        public eTeam Team;
        public bool PieceBecomeKing;
    }

    public class CheckersGame
    {
        //Constants
        private const byte k_KingScore = 4;
        private const byte k_RegularScore = 1;

        //Events
        public event CellChangeEventHandler CellChanged;

        public event PieceBecameKingEventHandler PieceBecameKing;

        public event PieceEatenEventHandler PieceEaten;

        //Member fields
        private static GameBoard s_GameBoard;
        private static eTeam s_Turn = eTeam.Black;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_Winner = null;
        private List<Movement> m_CurrentValidMoves = new List<Movement>();

        /*
        *  CheckersGame constructor for initilize the game.
        */
        public CheckersGame(byte i_BoardSize, string i_Player1Name, string i_Player2Name, bool i_IsPlayer2AI)
        {
            s_GameBoard = new GameBoard(i_BoardSize);
            m_Player1 = new Player(i_Player1Name, false, eTeam.Black);
            m_Player2 = new Player(i_Player2Name, i_IsPlayer2AI, eTeam.White);
        }


        //--------------internal method is responsible for raising the event--------------

        protected virtual void OnCellChanged(CellChangeEventArgs e)
        {
            CellChanged?.Invoke(e);
        }

        protected virtual void OnPieceBecameKing(CellChangeEventArgs e)
        {
            PieceBecameKing?.Invoke(e);
        }

        protected virtual void OnPieceEaten(CellChangeEventArgs e)
        {
            PieceEaten?.Invoke(e);
        }

        //------------------------------------Proporties--------------------------------------------

        /*
        * Get the game board.
        */
        public GameBoard GameBoard
        {
            get
            {
                return s_GameBoard;
            }
        }

        /*
        * Get the player that win the game.
        */
        public Player Winner
        {
            get
            {
                return m_Winner;
            }

            set
            {
                m_Winner = value;
            }
        }

        /*
         * Get the player1 .
         */
        public Player Player1
        {
            get
            {
                return m_Player1;
            }
        }

        /*
         * Get the player2 .
         */
        public Player Player2
        {
            get
            {
                return m_Player2;
            }
        }

        //---------------------------------Public functions--------------------------------

        /*
         * InitializeGame is a method that initialize the game.
         */
        public void InitializeGame()
        {
            m_Player1.Pieces.Clear();
            m_Player2.Pieces.Clear();
            initializeStartPositionOfPiecesOnBoard();
            m_Winner = null;
            s_Turn = eTeam.Black;
            buildCurrentValidMovesList();
        }

        /*
         * ChangeTurn is a method that change player turn.
         */
        public void ChangeTurn()
        {
            s_Turn = s_Turn == eTeam.Black ? eTeam.White : eTeam.Black;
            buildCurrentValidMovesList();
        }

        /*
         * GetCurrentPlayer is a method that return the current player that play.
         */
        public Player GetCurrentPlayer()
        {
            return s_Turn == m_Player1.r_Team ? m_Player1 : m_Player2;
        }

        /*
         * GetOpposingPlayer is a method that returns the opposing player of the player that this turn is his turn.
         */
        public Player GetOpposingPlayer()
        {
            return s_Turn == m_Player1.r_Team ? m_Player2 : m_Player1;
        }

        /*
         * GetOpposingPlayer is a method that recive player and returns the opposing player of him.
         */
        public Player GetOpposingPlayer(Player i_Player)
        {
            return i_Player == m_Player1 ? m_Player2 : m_Player1;
        }

        /*
         * ExecutePlayerAction is a method that executes player action by moving the piece,
         * derives if same piece can dominate again.
         */
        public void ExecutePlayerAction(Coordinates i_From, Coordinates i_To, out bool o_CanDominateAgain)
        {
            movePiece(i_From, i_To, out o_CanDominateAgain);
        }

        /*
         * UpdatePlayerScore is a method that calculates and updates winners score.
         */
        public void UpdatePlayerScore()
        {
            if (m_Winner != null)
            {
                Player opposingPlayer = GetOpposingPlayer(m_Winner);
                int score = calculateScore(m_Winner.GetNumberOfKings(), m_Winner.GetNumberOfRegularPieces());
                score -= calculateScore(opposingPlayer.GetNumberOfKings(), opposingPlayer.GetNumberOfRegularPieces());

                m_Winner.Score += score;
            }
        }

        /*
         * ExecuteComputerAction is a method that executes computer action by moving the piece, derives if same piece can dominate again.
         */
        public void ExecuteComputerAction()
        {
            bool canDominateAgain;

            doComputerMove(out canDominateAgain);

            while(canDominateAgain)
            {
                doComputerMove(out canDominateAgain);
            }

            ChangeTurn();
        }

        //---------------------------------Checking functions------------------------------

        /*
         * IsValidPlayerMove is a method that validates player input move by finding it in valid moves list.
         */
        public bool IsValidPlayerMove(Coordinates i_From, Coordinates i_To)
        {
            return getMovementByCoordinates(i_From, i_To) != null;
        }

        /*
         * IsGameOver is a method that checks if game is over.
         */
        public bool IsGameOver()
        {
            bool isGameOver = false;
            Player currentPlayer = GetCurrentPlayer();
            if (currentPlayer.GetNumberOfPieces() == 0)
            {
                m_Winner = GetOpposingPlayer();
                isGameOver = true;
            }
            else if (m_CurrentValidMoves.Count == 0)
            {
                ChangeTurn();
                if (m_CurrentValidMoves.Count == 0)
                {
                    m_Winner = null;
                }
                else
                {
                    m_Winner = GetCurrentPlayer();
                }

                isGameOver = true;
            }

            return isGameOver;
        }

        //------------------------------Private functions----------------------------------

        /*
         * initializeStartPositionOfPiecesOnBoard is a method that sorts pieces on board.
         */
        private void initializeStartPositionOfPiecesOnBoard()
        {
            byte numOfOccupiedRowsOnOneSide = (byte)((s_GameBoard.r_Size / 2) - 1);
            s_GameBoard.ClearBoard();
            initiazeOneSide(0, numOfOccupiedRowsOnOneSide, eTeam.White);
            initiazeOneSide((byte)(numOfOccupiedRowsOnOneSide + 2), s_GameBoard.r_Size, eTeam.Black);
        }

        /*
         * initiazeOneSide is a method that sorts one side of board.
         */
        private void initiazeOneSide(byte i_StartRow, byte i_EndRow, eTeam i_Team)
        {
            for (int row = i_StartRow; row < i_EndRow; row++)
            {
                int startColumn = row % 2 == 0 ? 1 : 0;
                for (int column = startColumn; column < s_GameBoard.r_Size; column += 2)
                {
                    Coordinates currentCoordinates = new Coordinates(column, row);
                    Piece currentPiece = new Piece(i_Team, currentCoordinates);
                    s_GameBoard[currentCoordinates] = currentPiece;

                    if (m_Player1.r_Team == i_Team)
                    {
                        m_Player1.AddPiece(currentPiece);
                    }
                    else if (m_Player2.r_Team == i_Team)
                    {
                        m_Player2.AddPiece(currentPiece);
                    }
                }
            }
        }

        /*
         * buildCurrentValidMovesList is a method that builds a list of current players available moves.
         */
        private void buildCurrentValidMovesList()
        {
            Player currentPlayer = GetCurrentPlayer();
            bool[] isDominationMove = new bool[4];
            bool needToRemoveInvalidMoves = false;

            if (m_CurrentValidMoves.Count != 0)
            {
                m_CurrentValidMoves.Clear();
            }

            foreach (Piece currentPlayerPiece in currentPlayer.Pieces)
            {
                Coordinates from = currentPlayerPiece.CoordinatesOnBoard;

                Coordinates upperLeft = new Coordinates(from.X - 1, from.Y - 1);
                Coordinates upperRight = new Coordinates(from.X + 1, from.Y - 1);
                Coordinates lowerLeft = new Coordinates(from.X - 1, from.Y + 1);
                Coordinates lowerRight = new Coordinates(from.X + 1, from.Y + 1);

                addValidMove(from, upperLeft, out isDominationMove[0]);
                addValidMove(from, upperRight, out isDominationMove[1]);
                addValidMove(from, lowerLeft, out isDominationMove[2]);
                addValidMove(from, lowerRight, out isDominationMove[3]);

                // if at least one of the moves is a domination move, remove all other non domination moves:
                if (isDominationMove[0] || isDominationMove[1] || isDominationMove[2] || isDominationMove[3])
                {
                    needToRemoveInvalidMoves = true;
                }
            }

            if (needToRemoveInvalidMoves)
            {
                removeNotDominationMovesFromList();
            }
        }

        /*
         * addValidMove is a method that adds a valid move to moves list.
         */
        private void addValidMove(Coordinates i_From, Coordinates i_To, out bool o_IsDominationMove)
        {
            Movement validMove = new Movement(i_From, i_To);

            if (validMove.IsValid())
            {
                m_CurrentValidMoves.Add(validMove);
                o_IsDominationMove = validMove.DominationMove;
            }
            else
            {
                o_IsDominationMove = false;
            }
        }

        /*
         * removeNotDominationMovesFromList is a method that removes all not domination moves from moves list.
         */
        private void removeNotDominationMovesFromList()
        {
            List<Movement> dominationMoveList = new List<Movement>();
            foreach (Movement currentMove in m_CurrentValidMoves)
            {
                if (currentMove.DominationMove)
                {
                    dominationMoveList.Add(currentMove);
                }
            }

            m_CurrentValidMoves = dominationMoveList;
        }

        /*
         * removeAllMovesNotRelatedToPiece is a method that removes all moves that not related to the given piece.
         */
        private void removeAllMovesNotRelatedToPiece(Piece i_Piece)
        {
            List<Movement> specificMovesList = new List<Movement>();
            Coordinates pieceCoordinates = i_Piece.CoordinatesOnBoard;
            foreach (Movement currentMove in m_CurrentValidMoves)
            {
                if (currentMove.From.EqualsTo(pieceCoordinates))
                {
                    specificMovesList.Add(currentMove);
                }
            }

            m_CurrentValidMoves = specificMovesList;
        }

        /*
         * movePiece is a method that relocates piece on board, derives if piece can dominate again.
         */
        private void movePiece(Coordinates i_From, Coordinates i_To, out bool o_CanDominateAgain)
        {
            Piece currentPiece = s_GameBoard[i_From];
            Movement currentMove = getMovementByCoordinates(i_From, i_To);

            s_GameBoard[currentMove.To] = currentPiece;
            currentPiece.CoordinatesOnBoard = currentMove.To;
            s_GameBoard.ClearCell(currentMove.From);

            if (currentMove.DominationMove)
            {
                dominatePiece(currentMove.DominatedPiece);

                o_CanDominateAgain = canDominateAgain(currentPiece);
            }
            else
            {
                o_CanDominateAgain = false;
            }

            CellChangeEventArgs arguments = new CellChangeEventArgs();
            arguments.From = i_From;
            arguments.To = i_To;
            arguments.Team = GetCurrentPlayer().r_Team;
            OnCellChanged(arguments);

            if (isPieceOnUpperOrLowerBoardEdge(currentPiece))
            {
                currentPiece.MakeKing();
                arguments.PieceBecomeKing = true;
                OnPieceBecameKing(arguments);
            }
        }

        /*
         * dominatePiece is a method that removes dominated piece from game.
         */
        private void dominatePiece(Piece i_DominatedPiece)
        {
            int row = i_DominatedPiece.CoordinatesOnBoard.Y;
            int column = i_DominatedPiece.CoordinatesOnBoard.X;

            Player opposingPlayer = GetOpposingPlayer();
            s_GameBoard.ClearCell(i_DominatedPiece.CoordinatesOnBoard);
            opposingPlayer.RemovePiece(i_DominatedPiece);

            CellChangeEventArgs arguments = new CellChangeEventArgs();
            arguments.EatenPiece = new Coordinates(column, row);
            arguments.Team = opposingPlayer.r_Team;

            OnPieceEaten(arguments);
        }

        /*
         * calculateScore is a method that calculate the score according to the
         * the following formula:
         * [(number of kings) * (king score)] + [(number of regular pieces) * (regular pieces score)]
         */
        private int calculateScore(byte i_NumberOfKings, byte i_NumberOfRegularPieces)
        {
            return (i_NumberOfKings * k_KingScore) + (i_NumberOfRegularPieces * k_RegularScore);
        }

        /*
         * getMovementByCoordinates is a method that derives movement from coordinates.
         */
        private Movement getMovementByCoordinates(Coordinates i_From, Coordinates i_To)
        {
            Movement returnValue = null;

            foreach (Movement validMove in m_CurrentValidMoves)
            {
                if (doesMovementContainCoordinates(validMove, i_From, i_To))
                {
                    returnValue = validMove;
                    break;
                }
            }

            return returnValue;
        }

        /*
         * makeRandomMove is a method that make a random game move.
         */
        private void makeRandomMove(out Coordinates o_From, out Coordinates o_To)
        {
            Random randomMoveIndex = new Random();
            int index = randomMoveIndex.Next(m_CurrentValidMoves.Count);
            o_From = m_CurrentValidMoves[index].From;
            o_To = m_CurrentValidMoves[index].To;
        }

        /*
         * doComputerMove is a method that execute computer move and return in the output parameter
         * if the computer can domain again in this turn.
         */
        private void doComputerMove(out bool o_CanDominateAgain)
        {
            Coordinates from;
            Coordinates to;
            makeRandomMove(out from, out to);
            movePiece(from, to, out o_CanDominateAgain);       
        }

        //----------------------------------Checking functions------------------------------

        /*
         * isPieceOnUpperOrLowerBoardEdge is a method that checks if current piece 
         * on upper or lower board edge.
         */
        private bool isPieceOnUpperOrLowerBoardEdge(Piece i_Piece)
        {
            return (i_Piece.r_Team == eTeam.Black && i_Piece.CoordinatesOnBoard.Y == 0) || (i_Piece.r_Team == eTeam.White && i_Piece.CoordinatesOnBoard.Y == GameBoard.r_Size - 1);
        }

        /*
         * canDominateAgain is a method that checks if current dominating piece can dominate 
         * another piece in the next turn.
         */
        private bool canDominateAgain(Piece i_Piece)
        {
            Coordinates PieceCoordinates = i_Piece.CoordinatesOnBoard;
            bool canDominateAgain = false;

            buildCurrentValidMovesList();
            if (m_CurrentValidMoves.Count != 0)
            {
                if (m_CurrentValidMoves[0].DominationMove)
                {
                    foreach (Movement currentMove in m_CurrentValidMoves)
                    {
                        if (currentMove.From.EqualsTo(PieceCoordinates))
                        {
                            canDominateAgain = true;
                            break;
                        }
                    }

                    if (canDominateAgain)
                    {
                        removeAllMovesNotRelatedToPiece(i_Piece);
                    }
                }
            }

            return canDominateAgain;
        }

        /*
         * doesMovementContainCoordinates is a method that checks if a movement contains given coordinates.
         */
        private bool doesMovementContainCoordinates(Movement i_Move, Coordinates i_From, Coordinates i_To)
        {
            return i_Move.From.EqualsTo(i_From) && i_Move.To.EqualsTo(i_To);
        }

        ////-------------------------------------------------------------------------------
        //                             Nested movement class
        ////-------------------------------------------------------------------------------

        public class Movement
        {
            //Member fields
            private Coordinates m_From;
            private Coordinates m_To;
            private Coordinates m_MoveDirection;
            private bool m_DominationMove = false;
            private Piece m_DominatedPiece = null;

            /*
            *  Movement constructor.
            */
            public Movement(Coordinates i_From, Coordinates i_To)
            {
                m_From = i_From;
                m_To = i_To;
                calculateMoveDirection();
            }

            //------------------------------------Proporties--------------------------------------------

            /*
             * Get or set the From coordinate.
             */
            public Coordinates From
            {
                get
                {
                    return m_From;
                }

                set
                {
                    m_From = value;
                    calculateMoveDirection();
                }
            }

            /*
             * Get or set the To coordinate.
             */
            public Coordinates To
            {
                get
                {
                    return m_To;
                }

                set
                {
                    m_To = value;
                    calculateMoveDirection();
                }
            }

            /*
             * Get the DominationMove.
             */
            public bool DominationMove
            {
                get
                {
                    return m_DominationMove;
                }
            }

            /*
             * Get the DominatedPiece.
             */
            public Piece DominatedPiece
            {
                get
                {
                    return m_DominatedPiece;
                }
            }

            // ----------------------Validation check functions-------------------------

            /*
             *  IsValid is a method that checks if movement is valid.
             */
            public bool IsValid()
            {
                bool isValid = false;

                if (s_GameBoard.DoesCellExist(m_From) && s_GameBoard.DoesCellExist(m_To))
                {
                    if (s_GameBoard.IsOccupiedCell(m_From) && isValidCheckersPieceMovement())
                    {
                        Piece currentPiece = s_GameBoard[m_From];

                        if (isTeamsTurn(currentPiece))
                        {
                            isValid = !currentPiece.IsKing ? isCorrectMovementDirection() : true;

                            if (isValid)
                            {
                                if (s_GameBoard.IsOccupiedCell(m_To))
                                {
                                    isValid = tryDomination();
                                }
                            }
                        }
                    }
                }

                return isValid;
            }

            /*
             *  calculateMoveDirection is a method that calculates the direction of movement, returns  direction coordinates.
             */
            private void calculateMoveDirection()
            {
                sbyte rowDirection = (sbyte)(m_To.Y - m_From.Y);
                sbyte columnDirection = (sbyte)(m_To.X - m_From.X);
                m_MoveDirection = new Coordinates(columnDirection, rowDirection);
            }

            /*
             *  isValidCheckersPieceMovement is a method that validates if piece was moves horizontaly to an adjacent cell.
             */
            private bool isValidCheckersPieceMovement()
            {
                byte rowMovement = (byte)Math.Abs(m_MoveDirection.Y);
                byte colMovement = (byte)Math.Abs(m_MoveDirection.X);

                return (rowMovement == colMovement) && (rowMovement == 1);
            }

            /*
             *  isTeamsTurn is a method that validates if piece is moved in its turn.
             */
            private bool isTeamsTurn(Piece i_CurrentPeace)
            {
                return i_CurrentPeace.r_Team == s_Turn;
            }

            /*
             *  isCorrectMovementDirection is a method that validates the direction of the movement.
             */
            private bool isCorrectMovementDirection()
            {
                return (s_Turn == eTeam.Black && m_MoveDirection.Y < 0) || (s_Turn == eTeam.White && m_MoveDirection.Y > 0);
            }

            /*
             *  tryDomination is a method that checks if a piece jump over another and dominate it, saves the dominated piece.
             */
            private bool tryDomination()
            {
                bool canDominate = false;
                Piece currentPiece = s_GameBoard[m_From];
                Piece dominationCandidate = s_GameBoard[m_To];

                if (dominationCandidate.r_Team != currentPiece.r_Team)
                {
                    Coordinates jumpTo = new Coordinates(m_MoveDirection.X + m_To.X, m_MoveDirection.Y + m_To.Y);

                    if (s_GameBoard.DoesCellExist(jumpTo))
                    {
                        if (!s_GameBoard.IsOccupiedCell(jumpTo))
                        {
                            m_DominatedPiece = dominationCandidate;
                            To = jumpTo;
                            m_DominationMove = true;
                            canDominate = true;
                        }
                    }
                }

                return canDominate;
            }
        }
    }
}